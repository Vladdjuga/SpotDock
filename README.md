# SpotDock – Spot Container Marketplace

A playground implementation of a **spot-market for container compute** – think "AWS Spot Instances" or "Ebay for Docker containers".

Users submit **bids** like:

> "I want 2 vCPU / 4 GiB RAM and will pay $0.05 per minute"

The system maintains a limited pool of simulated servers and **continuously matches** bids to available capacity. Higher-paying bids preempt lower-paying ones; containers can be **evicted** when the market price moves.

This repo is built to explore **high-concurrency matching, distributed sagas, and real Docker/Kubernetes orchestration** from .NET.

---

## Core Concepts

- **Bid**: User request for compute capacity (CPU, RAM, max price per time unit, image, TTL, etc.).
- **Resource / Capacity**: Abstraction over available compute nodes (simulated server pool, real Docker host, or Kubernetes cluster).
- **Matching Engine**: Continuously matches bids against available capacity using a Redis-backed order book.
- **Container Allocation**: When a bid is matched, the system starts a container via Docker API / Kubernetes API.
- **Lifecycle Saga**: MassTransit saga that tracks bid & container lifecycle: Submitted → Matched → Running → Evicted / Completed / Cancelled.
- **Billing**: Per‑second (or per‑N‑seconds) metering of running containers, charging the user wallet.
- **Eviction Logic**: If higher bids appear and capacity is constrained, lower-priced containers are stopped and resources are reassigned.

---

## Solution Structure

- `SpotDock.Web` – ASP.NET Core entrypoint, HTTP API, minimal UI, DI composition root.
- `SpotDock.Modules.Auctions` – Bids, order book, matching engine, domain events.
- `SpotDock.Modules.Billing` – Wallets, per‑second billing, invoices, cost tracking.
- `SpotDock.Modules.Compute` – Abstraction over compute backends (Docker / Kubernetes), container lifecycle, capacity model.
- `SpotDock.Shared.Kernel` – Shared domain primitives (value objects, base entities, result types, integration event contracts, etc.).
- `SpotDock.Tests` – Unit and integration tests.

Module boundaries are intentionally explicit to keep **Auctions**, **Billing**, and **Compute** loosely coupled.

---

## Technology Stack

- **Runtime**: .NET 10 LTS.
- **Web**: ASP.NET Core minimal hosting model.
- **Messaging / Sagas**: MassTransit with a saga state machine for bid/container lifecycle.
- **State / Order Book**: Redis for fast price levels, bid queues, and distributed locks.
- **Compute Backend**:
  - Phase 1: Docker Engine API (via Docker Desktop or remote Docker host).
  - Phase 2 (optional): Kubernetes API integration.
- **Data Storage**: Relational DB for durable state (PostgreSQL or SQL Server – TBD).

---

## High-Level Flows

### 1. Bid Submission

1. User submits a bid via `SpotDock.Web` (HTTP API or UI).
2. Request is validated and stored via `SpotDock.Modules.Auctions`.
3. A `BidSubmitted` event is published.
4. Matching engine picks up the bid and tries to allocate capacity.

### 2. Matching & Allocation

1. Matching engine reads the **order book** from Redis.
2. If capacity is available, bid is matched and a `BidMatched` event is raised.
3. A saga triggers container creation in `SpotDock.Modules.Compute`.
4. Docker/Kubernetes creates the container; its ID and metadata are persisted.
5. Saga transitions to **Running** state.

### 3. Billing

1. Billing service periodically queries running containers (or consumes events on start/stop).
2. Computes usage in seconds (or fixed time slices) using start/stop timestamps.
3. Deducts funds from user wallet; may emit `InsufficientFunds` events.
4. If balance is insufficient, billing asks compute module to stop containers.

### 4. Eviction

1. When new, higher-priced bids appear and capacity is full, auctions module determines
   which low-priced bids/containers must be evicted.
2. Eviction decisions are published as events (e.g. `ContainerEvictionRequested`).
3. Compute module stops containers and frees capacity.
4. Saga transitions to **Evicted**, billing closes out final charges.

---

## Local Development (Planned)

> This section describes the intended setup; details may change as the implementation evolves.

### Prerequisites

- .NET SDK (preview that supports .NET 10, or latest stable if the target is adjusted).
- Docker Desktop (or another Docker engine reachable from the host).
- Redis instance (Docker container is fine).
- (Optional) PostgreSQL / SQL Server for durable state.

### Basic Run

From the repo root:

```powershell
# Restore and build
dotnet restore
 dotnet build

# Run web entrypoint
cd SpotDock.Web
 dotnet run
```

Then open the printed URL (typically `https://localhost:5001` or similar) to access the API/UI.

---

## Design Goals

- **Realistic, not production**: Explore real-world patterns (sagas, spot markets, eviction) without the overhead of production-hardening everything.
- **Concurrency-focused**: Embrace race conditions, contention, and failure modes as first-class design problems.
- **Extensible**: Make it easy to plug in different compute backends or pricing strategies.
- **Testable**: Core logic for matching, billing, and eviction should be covered by tests.

---

## Roadmap / TODO

Status legend:
- `✅` – Done
- `🟡` – In progress
- `🧠` – Thinking / design stage
- `🔜` – Planned / not started

### Foundation & Infrastructure

- 🟡 Solution skeleton with projects:
  - `SpotDock.Web`
  - `SpotDock.Modules.Auctions`
  - `SpotDock.Modules.Billing`
  - `SpotDock.Modules.Compute`
  - `SpotDock.Shared.Kernel`
  - `SpotDock.Tests`
- 🔜 Add Docker Compose for Redis + DB + web app.
- 🔜 Configure logging, metrics, and basic health checks.

### Domain Modeling

- ✅ Define core domain model for **Bid**, **Resource**, **ContainerAllocation**.
- 🟡 Model spot-pricing and order-book abstraction (price levels, FIFO queues per level).
- 🧠 Decide on units and precision for pricing (per-second vs per-minute, decimal precision, currency handling).
- 🔜 Define user wallet / account model for billing.

### Matching Engine

- 🟡 Implement Redis-backed order book with:
  - price levels as sorted sets
  - per-level queues for bids
- 🔜 Implement matching algorithm (best price wins, FIFO within same price).
- 🔜 Handle race conditions: bid cancel vs. allocation, concurrent matches for same capacity.
- 🧠 Explore optimistic locking vs distributed locks (e.g., Redis RedLock) around capacity updates.

### Sagas & Messaging

- 🟡 Introduce MassTransit and configure transport (RabbitMQ, Azure Service Bus, or in-memory for dev).
- 🔜 Implement **BidLifecycleSaga**:
  - Submitted → Matched → Running → Completed / Evicted / Cancelled
- 🔜 Implement **BillingSaga** or scheduled billing jobs based on container lifecycle events.
- 🧠 Consider outbox pattern for integration events across modules.

### Compute Module (Docker / Kubernetes)

- 🟡 Define abstraction for compute backend (`IComputeProvider` or similar).
- 🔜 Implement Docker Engine provider:
  - Start container with resource constraints
  - Stop/kill container
  - Query container state
- 🧠 Design Kubernetes provider:
  - Map bids to Pod specs
  - Support eviction via pod deletion or taints/tolerations.

### Billing & Wallets

- 🟡 Define wallet and transaction domain model.
- 🔜 Implement per-second (or tick-based) billing service using MassTransit scheduled messages or background service.
- 🔜 Integrate with compute events (container started/stopped/evicted).
- 🧠 Explore different billing strategies (prepaid vs postpaid, max budget per bid, etc.).

### Eviction Logic

- 🟡 Specify eviction policies:
  - Outbid-only eviction (only when higher price appears)
  - Grace periods / notifications (if implemented in UI/API)
- 🔜 Implement eviction decision engine in Auctions module.
- 🔜 Integrate eviction with Compute module and sagas.
- 🧠 Model user-facing experience around eviction (events, webhooks, notifications).

### API / Web

- 🟡 Minimal HTTP API to submit bids and query state.
- 🔜 Simple web UI or Swagger UI for manual testing.
- 🔜 Authentication/authorization (even minimal) for multi-user scenarios.
- 🧠 Consider exposing metrics and a simple market dashboard (current price, utilization).

### Testing & Simulation

- 🟡 Unit tests for:
  - matching engine
  - eviction ranking
  - billing calculation
- 🔜 Integration tests with Redis and Docker (behind test flags or profiles).
- 🧠 Build simulation scripts to stress-test matching under load (many bids, limited capacity).

---

## Contributing / Next Steps

This project is currently experimental and focused on learning and exploration. The immediate next steps are:

1. Finalize the domain model for bids, capacity, and billing.
2. Implement a minimal vertical slice: submit bid → match → start Docker container → bill → stop.
3. Add monitoring/metrics to observe how the market behaves under load.

If you want to extend or adjust anything (e.g., different pricing models, alternative message brokers, Kubernetes support), keep module boundaries clean and update this README to reflect the new design.

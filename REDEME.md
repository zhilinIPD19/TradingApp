# Trading Application (WPF + .NET + SignalR)

## Overview

This project is a real-time trading simulation application built with WPF and ASP.NET Core.

It demonstrates:

* Real-time updates using SignalR
* Reliable API communication with Polly (retry, timeout)
* Idempotent order placement to prevent duplicate trades
* MVVM architecture in WPF

---

## Setup

### Prerequisites

* .NET 8 SDK
* Visual Studio 2022+
* Windows (for WPF)

---

## ▶️ How to Run

### 1. Start Backend API

* Set **TradingApi** as startup project
* Run the project
* Swagger should open:

```
https://localhost:7007/swagger
```

---

### 2. Start WPF Client

* Set **TradingUI** as startup project
* Run the project

---

### 3. Verify Connection

* The UI should load accounts and positions
* Real-time updates will appear automatically via SignalR

---

## Architecture

### High-Level Design

```
WPF UI (MVVM)
   ↓
TradingApiService (HttpClient + Polly)
   ↓
ASP.NET Core API
   ↓
In-memory Data Store
   ↓
SignalR Hub (real-time updates)
```

---

### Key Components

#### Frontend (WPF)

* MVVM pattern
* ObservableCollection for UI updates
* Data binding with INotifyPropertyChanged

#### API Layer

* REST endpoints for:

  * Accounts
  * Positions
  * Orders

#### Real-Time (SignalR)

* Push updates for:

  * Positions
  * Account balance
* Automatic reconnect enabled

#### Resilience (Polly)

* Retry with exponential backoff
* Timeout handling

#### Idempotency

* Each order includes an `Idempotency-Key`
* Prevents duplicate order execution on retries

---

## Features

* Account & Position view (Master-Detail layout)
* Real-time updates via SignalR
* PnL calculation and UI updates
* Cash Balance change highlighting (visual feedback)
* Order placement with validation
* Retry-safe order execution (Idempotency)

---

## Tech Stack

* **Frontend**: WPF (.NET)
* **Backend**: ASP.NET Core Web API
* **Real-time**: SignalR
* **Resilience**: Polly
* **Architecture**: MVVM

---

## Notes

* This project uses in-memory data for simplicity
* Designed to simulate a real trading system architecture
* Can be extended with:

  * Database persistence
  * Authentication
  * Redis for distributed idempotency

---

## Future Improvements

* Add persistent storage (SQL / NoSQL)
* Implement distributed idempotency (Redis)
* Add authentication & authorization
* Improve UI styling (dark mode, charts)

---

## Author

Zhilin Lin

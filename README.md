# Computation Engine with CSV-Based Function Mappings

## 📌 Overview
This **Computation Engine** dynamically loads a **Directed Acyclic Graph (DAG)** from a **CSV file** and maps **functions** to computation nodes. The engine efficiently executes the graph **concurrently** using `ThreadPool`, ensuring **high-performance parallel execution**.

### 🚀 **Key Features**
- **DAG-based execution** → Defines dependencies dynamically.
- **CSV-based configuration** → Functions and dependencies are external.
- **No Reflection Overhead** → Uses pre-registered delegates for performance.
- **Multi-threaded Execution** → Uses `ThreadPool` with `SemaphoreSlim`.
- **Separation of Concerns** → Modular design allows independent function development.


---

## 📂 **Project Structure**
```
.
├── ComputationEngine
│   ├── Program.cs # Main execution logic
│   ├── ComputationEngine.cs # DAG execution with parallel processing
│   ├── Functions.cs # All computation functions
│   ├── FunctionsRegistry.cs # Pre-registered function mappings
│   ├── dependencies.csv # Graph structure (edges)
│   └── functions.csv # Function mappings for each node
└── README.md
```

---

## **🛠 Scalability & Modular Design**
### **🔹 Separation of Concerns**
- The **Computation Engine** is responsible for **dependency resolution and execution**.
- The **Function Registry** provides a **clean API for function execution**, avoiding reflection.
- The **Function Implementations** (`Functions.cs`) are **completely decoupled** from execution logic.

### **🔹 Independent Function Development**
- Each function **operates independently** and can be **developed, tested, and optimized separately**.
- Functions **only access precomputed values**, ensuring correctness.
- **New functions can be added** without modifying the computation engine.

### **🔹 Concurrency & Read-Only Input for Safety**
- Each function receives a **`ConcurrentDictionary<string, object>` (read-only access)**.
- A function **can only use values that have already been computed**.
- Functions **cannot modify shared state**, preventing race conditions.

### **🔹 Scaling to Large Graphs**
- Supports **thousands of computation nodes** efficiently.
- Uses **lock-free** **`ConcurrentDictionary`** for **fast read/write access**.
- The **topological execution order** ensures correctness.

---

## 📄 **Configuration Files**

### **🔹 `dependencies.csv` (Graph Structure)**
Defines **node dependencies** (`From` → `To`).

- `A → C` (Node A is required before C)
- `B → C`, `C → D`, etc.

---

### **🔹 `functions.csv` (Function Mappings)**
Maps **graph nodes** to **specific computation functions**.

- `A` calculated by `FetchFromDatabase()`
- `B` calculated by `FetchFromAPI()`, etc.

---

### **🔹 `FunctionsRegistry.cs` (Function Registry Lookups)**
Maps **function name** to **specific computation functions**.

- `"FetchFromDatabase"` runs `FetchFromDatabase()`
- `"FetchFromApi"` runs `FetchFromAPI()`, etc.

---

## 🚀 **Setup & Execution**
### **1️⃣ Clone the Repository**
```sh
git clone https://github.com/xubinzha/ComputationEngine.git
cd ComputationEngine

```
### **2️⃣ Build and Run**
```sh
dotnet build
dotnet run
```

---

## 📌 Sample Output
```rust
A (String): DB Result for Customer 123
B (String): API Response: Dummy Data
C (String): Computed: DB Result for Customer 123 | API Response: Dummy Data
D (String): COMPUTED: DB RESULT FOR CUSTOMER 123 | API RESPONSE: DUMMY DATA
E (Boolean): False
F (String): Final Result: False
```

---

## 📌 Performance Tests
Randomly generate a graph with **1000** nodes, each node with maximum **5** dependencies. Compare the **execution time** among **sequential**, **BFS single-thread** and **multi-thread (maxThread = 10)** engine.
```aiignore
🔴 Sequential Execution Time: 15055 ms, with 1000 nodes
🔴 Single-Threaded Execution Time: 2523 ms, with 1000 nodes
🟢 Multi-Threaded Execution Time: 263 ms, with 1000 nodes

```

---

## ⚡ How to Extend the System
| Change               | What to Do                                                                           |
|----------------------|--------------------------------------------------------------------------------------|
| Add new functions    | Define in `Functions.cs`, register in `FunctionsRegistry.cs`, map in `functions.csv` |
| Modify dependencies  | Edit `dependencies.csv`                                                              |
| Increase concurrency | Adjust `SemaphoreSlim` in `ComputationEngine.cs`                                     |

## 🎯 Next Steps
- Graph visualization of execution order
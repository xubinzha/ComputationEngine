# Computation Engine with CSV-Based Function Mappings

## 📌 Overview
This **Computation Engine** dynamically loads a **Directed Acyclic Graph (DAG)** from a **CSV file** and maps **functions** to computation nodes. The engine efficiently executes the graph **concurrently** using `ThreadPool`, ensuring **high-performance parallel execution**.

### 🚀 **Key Features**
- **DAG-based execution** → Defines dependencies dynamically.
- **CSV-based configuration** → Functions and dependencies are external.
- **No Reflection Overhead** → Uses pre-registered delegates for performance.
- **Multi-threaded Execution** → Uses `ThreadPool` with `SemaphoreSlim`.

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

### **🔹 `FunctionsRegistry.cs` (Function Registry)**
Maps **function name** to **specific computation functions**.

- `"FetchFromDatabase"` runs `FetchFromDatabase()`
- `"FetchFromApi"` runs `FetchFromAPI()`, etc.

---

## 🚀 **Setup & Execution**
### **1️⃣ Clone the Repository**
```sh
git clone https://github.com/your-repo/computation-engine.git
cd computation-engine

```
### **2️⃣ Build and Run**
```sh
dotnet build
dotnet run
```

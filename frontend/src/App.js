import React, { useState, useEffect } from "react";
import api from "./components/apiConfig";
import "./App.css";

function App() {
  const [algorithms, setAlgorithms] = useState([]);
  const [testingFunctions, setTestingFunctions] = useState([]);

  const fetchAlgorithms = async () => {
    api
      .get("http://localhost:3001/api/get")
      .then((response) => {
        setAlgorithms(response.data);
      })
      .catch((error) => {
        setAlgorithms([
          { name: "a1", id: 0 },
          { name: "a2", id: 1 },
          { name: "a3", id: 2 },
        ]);
      });
  };
  const fetchTestingFunctions = async () => {
    api
      .get("http://localhost:3001/api/get")
      .then((response) => {
        console.log(response);
      })
      .catch((error) => {
        setTestingFunctions([
          { name: "f1", id: 0 },
          { name: "f2", id: 1 },
          { name: "f3", id: 2 },
        ]);
      });
  };

  const addAlgorithm = async () => {
    const result = await api.post("http://localhost:3001/api/insert", {
      name: "a4",
    });
    console.log(result);
  };

  const renderAlgorithms = () => {
    return algorithms.map((algorithm) => {
      return (
        <div className="algorithm" key={algorithm.id}>
          <h2>
            {algorithm.name}
            <input type="checkbox" />
          </h2>
        </div>
      );
    });
  };
  const renderTestingFunctions = () => {
    return testingFunctions.map((testingFunction) => {
      return (
        <div className="testingFunction" key={testingFunction.id}>
          <h2>
            {testingFunction.name}
            <input type="checkbox" />
          </h2>
        </div>
      );
    });
  };

  useEffect(() => {
    fetchAlgorithms();
    fetchTestingFunctions();
  }, []);

  return (
    <div className="App">
      <section className="algorithms">
        <h2>Algorithms</h2>
        {renderAlgorithms()}
      </section>
      <section className="testingFunctions">
        <h2>Testing Functions</h2>
        {renderTestingFunctions()}
      </section>
      <button>Go</button>
    </div>
  );
}

export default App;

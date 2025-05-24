import React, { useEffect, useState } from "react";
import logo from "./logo.svg";
import "./App.css";
import { getWeatherForecast } from "./webapi";

function App() {
  const [data, setData] = useState<Array<any>>([]);
  const [error, setError] = useState(null);

  useEffect(() => {
    getWeatherForecast()
      .then((res) => {
        setData(res.data);
        console.log(res.data);
      })
      .catch((err) => {
        setError(err.message);
        console.log(err.message);
      });
  }, []);

  if (error) return <div>Error: {error}</div>;
  if (!data) return <div>Loading...</div>;

  return (
    <>
      <h1>React APP</h1>
      <h1>Weather Forecast</h1>
      <ul>
        {data.map((item) => (
          <li key={item.date}>
            {item.date}: {item.temperatureC}°C - {item.summary}
          </li>
        ))}
      </ul>
    </>
  );
}

export default App;

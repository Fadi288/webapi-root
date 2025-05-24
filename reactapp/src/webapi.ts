import axios from 'axios';

const api = axios.create({
  baseURL: process.env.REACT_APP_API_URL,  // Docker internal URL for your API
  // If needed, add headers, tokens, etc.
  withCredentials: true,
});

// export const getWeatherForecast = () => api.get('/weatherforecast');
export const getWeatherForecast = () => api.get('/weatherforecast');

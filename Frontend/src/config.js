const config = {
  apiUrl: process.env.NODE_ENV === 'production'
    ? 'https://url-shortening-service-1.onrender.com' // Production backend URL
    : 'http://localhost:5191'
};

export default config;
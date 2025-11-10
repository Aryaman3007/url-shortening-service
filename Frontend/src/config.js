const config = {
  apiUrl: process.env.NODE_ENV === 'production'
    ? 'https://your-backend-url.onrender.com' // Replace with your actual backend URL after deployment
    : 'http://localhost:5191'
};

export default config;
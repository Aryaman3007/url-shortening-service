import axios from 'axios';

const baseURL = process.env.REACT_APP_API_URL || 'http://localhost:5191';

const api = axios.create({
    baseURL,
    headers: {
        'Content-Type': 'application/json',
    }
});

export const urlService = {
    shortenUrl: (url) => api.post('/shorten', { url }),
    getUrlStats: (shortCode) => api.get(`/shorten/${shortCode}/stats`),
    updateUrl: (shortCode, url) => api.put(`/shorten/${shortCode}`, { url }),
    deleteUrl: (shortCode) => api.delete(`/shorten/${shortCode}`),
};

export default api;
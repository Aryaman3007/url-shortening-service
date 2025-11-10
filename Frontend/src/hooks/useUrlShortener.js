import { useState } from 'react';
import { urlService } from '../services/api';

export const useUrlShortener = () => {
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState(null);

    const shortenUrl = async (url) => {
        setLoading(true);
        setError(null);
        try {
            const response = await urlService.shortenUrl(url);
            return response.data;
        } catch (err) {
            setError(err.message);
            return null;
        } finally {
            setLoading(false);
        }
    };

    const getUrlStats = async (shortCode) => {
        setLoading(true);
        setError(null);
        try {
            const response = await urlService.getUrlStats(shortCode);
            return response.data;
        } catch (err) {
            setError(err.message);
            return null;
        } finally {
            setLoading(false);
        }
    };

    const updateUrl = async (shortCode, url) => {
        setLoading(true);
        setError(null);
        try {
            const response = await urlService.updateUrl(shortCode, url);
            return response.data;
        } catch (err) {
            setError(err.message);
            return null;
        } finally {
            setLoading(false);
        }
    };

    const deleteUrl = async (shortCode) => {
        setLoading(true);
        setError(null);
        try {
            await urlService.deleteUrl(shortCode);
            return true;
        } catch (err) {
            setError(err.message);
            return false;
        } finally {
            setLoading(false);
        }
    };

    return {
        shortenUrl,
        getUrlStats,
        updateUrl,
        deleteUrl,
        loading,
        error
    };
};
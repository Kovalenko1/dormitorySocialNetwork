import React from 'react';
import { Navigate } from 'react-router-dom';

export const PublicRoute = ({ children }) => {
    if (localStorage.getItem('user')) {
        return <Navigate to="/" />;
    }

    return children;
};
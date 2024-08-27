import React from 'react';
import { Navigate } from 'react-router-dom';

export const ProtectedRoute = ({ children }) => {
    if (!localStorage.getItem('isAuth')) {
        return <Navigate to="/auth" />;
    }

    return children;
};
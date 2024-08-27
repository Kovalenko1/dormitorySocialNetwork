import React from 'react';
import { Navigate } from 'react-router-dom';

export const PublicRoute = ({ children }) => {
    if (localStorage.getItem('isAuth')) {
        console.log(localStorage.getItem('isAuth'))
        return <Navigate to="/" />;
    }

    return children;
};
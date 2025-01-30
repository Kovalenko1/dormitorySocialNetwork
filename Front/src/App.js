import React, { Suspense, lazy } from 'react';
import { BrowserRouter as Router, Route, Routes } from 'react-router-dom';
import { ProtectedRoute } from './rotesCapability/ProtectedRoute';
import { PublicRoute } from "./rotesCapability/PublicdRoute";

const Auth = lazy(() => import('./components/Auth/Auth'));
const Messenger = lazy(() => import('./components/Messanger/Messenger'));
const Sidebar = lazy(() => import('./components/Sidebar/Sidebar'));

function App() {
    return (
        <Router>
            <Suspense fallback={<div>Загрузка...</div>}>
                <Routes>
                    <Route
                        path="/"
                        element={
                            <ProtectedRoute>
                                <main style={{ display: 'flex', flexDirection: 'row', height: '100vh', width: '100%' }}>
                                    <Sidebar />
                                    <Messenger />
                                </main>
                            </ProtectedRoute>
                        }
                    />
                    <Route
                        path="/auth"
                        element={
                            <PublicRoute>
                                <Auth />
                            </PublicRoute>
                        }
                    />
                </Routes>
            </Suspense>
        </Router>
    );
}

export default App;

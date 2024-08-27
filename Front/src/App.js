import {Auth} from "./components/Auth/Auth"
import {Messenger} from "./components/Messanger/Messenger";
import {Sidebar} from "./components/Sidebar/Sidebar";
import {BrowserRouter as Router, Route, Routes, Navigate} from 'react-router-dom';
import {ProtectedRoute} from './rotesCapability/ProtectedRoute';
import {PublicRoute} from "./rotesCapability/PublicdRoute";

function App() {
    return (
        <Router>
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
        </Router>);
}

export default App;

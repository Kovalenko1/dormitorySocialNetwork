import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { login, signup } from '../../store/slices/authSlice'; // <-- импортируем signup
import { useNavigate } from 'react-router-dom';
import styles from './Auth.module.scss';

export const Auth = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const { loading, error } = useSelector((state) => state.auth);

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const [signupData, setSignupData] = useState({
        username: '',
        email: '',
        password: ''
    });

    const handleSignupChange = (e) => {
        const { name, value } = e.target;
        setSignupData((prevData) => ({
            ...prevData,
            [name]: value,
        }));
    };

    const handleSignupSubmit = async (e) => {
        e.preventDefault();
        try {
            const resultAction = await dispatch(signup(signupData));
            if (signup.fulfilled.match(resultAction)) {
                navigate('/');
            }
        } catch (err) {
            console.error(err);
        }
    };

    const handleLoginSubmit = async (e) => {
        e.preventDefault();
        const resultAction = await dispatch(login({ email, password }));

        if (login.fulfilled.match(resultAction)) {
            navigate('/');
        }
    };

    return (
        <main className={styles.authForm}>
            <div className={styles.main}>
                <input type="checkbox" id={styles.chk} aria-hidden="true" />

                <div className={styles.signup}>
                    <form onSubmit={handleSignupSubmit}>
                        <label htmlFor={styles.chk} aria-hidden="true">Регистрация</label>
                        <input
                            type="text"
                            name="username"
                            placeholder="Ник"
                            required
                            value={signupData.username}
                            onChange={handleSignupChange}
                        />
                        <input
                            type="email"
                            name="email"
                            placeholder="Email"
                            required
                            value={signupData.email}
                            onChange={handleSignupChange}
                        />
                        <input
                            type="password"
                            name="password"
                            placeholder="Пароль"
                            required
                            value={signupData.password}
                            onChange={handleSignupChange}
                        />
                        <button type="submit" disabled={loading}>
                            {loading ? 'Регистрация...' : 'Зарегистрироваться'}
                        </button>
                    </form>
                    {error && <p style={{ color: 'red' }}>{error.title || "Произошла ошибка"}</p>}
                </div>

                <div className={styles.login}>
                    <form onSubmit={handleLoginSubmit}>
                        <label htmlFor={styles.chk} aria-hidden="true">Вход</label>
                        <input
                            type="email"
                            placeholder="Email"
                            required
                            value={email}
                            onChange={(e) => setEmail(e.target.value)}
                        />
                        <input
                            type="password"
                            placeholder="Пароль"
                            required
                            value={password}
                            onChange={(e) => setPassword(e.target.value)}
                        />
                        <button type="submit" disabled={loading}>
                            {loading ? 'Вход...' : 'Войти'}
                        </button>
                    </form>
                    {error && <p style={{ color: 'red' }}>{error.title || "Произошла ошибка"}</p>}
                </div>
            </div>
        </main>
    );
};

import React, { useState } from 'react';
import { useDispatch, useSelector } from 'react-redux';
import { login } from '../../store/slices/authSlice';
import { useNavigate } from 'react-router-dom';
import styles from './Auth.module.scss';
import axios from "axios";

export const Auth = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    const { loading, error } = useSelector((state) => state.auth);

    const [email, setEmail] = useState('');
    const [password, setPassword] = useState('');

    const handleLoginChange = (e) => {
        // const { name, value } = e.target;
        // setLoginData({ ...loginData, [name]: value });
    };

    const handleSignupChange = (e) => {
        // const { name, value } = e.target;
        // setSignupData({ ...signupData, [name]: value });
    };

    const handleLoginSubmit = async (e) => {
        e.preventDefault();
        const resultAction = await dispatch(login({ email, password }));

        if (login.fulfilled.match(resultAction)) {
            navigate('/');
        }
    };

    const handleSignupSubmit = async (e) => {
        // e.preventDefault();
        // dispatch(signup(signupData, navigate));
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
                            // value={signupData.username}
                            onChange={handleSignupChange}
                        />
                        <input
                            type="email"
                            name="email"
                            placeholder="Email"
                            required
                            // value={signupData.email}
                            onChange={handleSignupChange}
                        />
                        <input
                            type="password"
                            name="password"
                            placeholder="Пароль"
                            required
                            // value={signupData.password}
                            onChange={handleSignupChange}
                        />
                        <button type="submit">Зарегистрироваться</button>
                    </form>
                </div>

                <div className={styles.login}>
                    <form onSubmit={handleLoginSubmit}>
                        <label htmlFor={styles.chk} aria-hidden="true">Вход</label>
                        <input
                            type="email"
                            placeholder="Email"
                            required
                            value={email}
                            onChange={(e)=> setEmail(e.target.value)}
                        />
                        <input
                            type="password"
                            placeholder="Пароль"
                            required
                            value={password}
                            onChange={(e)=> setPassword(e.target.value)}
                        />
                        <button type="submit" disabled={loading}>{loading ? 'Вход...' : 'Войти'}</button>
                    </form>
                    {error && <p style={{color: 'red'}}>{error}</p>}
                </div>
            </div>
        </main>
    );
};

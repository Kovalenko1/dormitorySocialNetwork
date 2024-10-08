import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';
import { setChats } from './chatSlice';

export const login = createAsyncThunk(
    'auth/login',
    async (credentials,{ dispatch, rejectWithValue }, thunkAPI) => {
        try {
            const response = await axios.post('http://localhost:5000/api/auth/login', credentials);
            localStorage.setItem('user', JSON.stringify(response.data));
            dispatch(setChats(response.data.chats));
            return response.data;
        } catch (error) {
            return thunkAPI.rejectWithValue(error.response.data);
        }
    }
);

export const logout = createAsyncThunk('auth/logout', async () => {
    localStorage.clear();
});

const authSlice = createSlice({
    name: 'auth',
    initialState: {
        user: null,
        token: null,
        isAuthenticated: false,
        loading: false,
        error: null,
    },
    reducers: {
    },
    extraReducers: (builder) => {
        builder
            .addCase(login.pending, (state) => {
                state.loading = true;
                state.error = null;
            })
            .addCase(login.fulfilled, (state, action) => {
                state.loading = false;
                state.isAuthenticated = true;
                state.user = action.payload.user;
                state.token = action.payload.token;
            })
            .addCase(login.rejected, (state, action) => {
                state.loading = false;
                state.error = action.payload.message;
            })
            .addCase(logout.fulfilled, (state) => {
                state.isAuthenticated = false;
                state.user = null;
                state.token = null;
            });
    },
});

export default authSlice.reducer;
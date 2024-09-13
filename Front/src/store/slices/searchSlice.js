import { createSlice, createAsyncThunk } from '@reduxjs/toolkit';
import axios from 'axios';

export const searchUsers = createAsyncThunk(
    'searchUsers',
    async (query) => {
        if (query) {
            const response = await axios.get(`http://localhost:5000/api/Search/usersSearch?query=${query}`);
            return response.data;
        }
        else {
            return [];
        }

    }
);

const searchSlice = createSlice({
    name: 'users',
    initialState: { list: [], status: null },
    reducers: {},
    extraReducers: (builder) => {
        builder.addCase(searchUsers.fulfilled, (state, action) => {state.list = action.payload;})

    },
});

export default searchSlice.reducer;

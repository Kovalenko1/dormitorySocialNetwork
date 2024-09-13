import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { connectToChatHub as connectService, joinPrivateChat as joinService, sendMessageToPrivateChat as sendService, getConnection } from '../../services/chatService';

const initialState = {
    connectionStatus: 'disconnected',
    messages: {},
    currentChatRoom: null,
    status: 'idle',
    error: null,
};

export const connectToChatHub = createAsyncThunk(
    'chat/connectToChatHub',
    async (_, { dispatch, rejectWithValue }) => {
        try {
            const result = await connectService(dispatch, receiveMessage);
            return result;
        } catch (error) {
            return rejectWithValue('Connection failed');
        }
    }
);

export const joinPrivateChat = createAsyncThunk(
    'chat/joinPrivateChat',
    async ({ userId1, userId2 }, { getState, rejectWithValue }) => {
        try {
            await joinService(userId1, userId2);
            return `${userId1}-${userId2}`;
        } catch (error) {
            return rejectWithValue(error.message);
        }
    }
);

export const sendMessageToPrivateChat = createAsyncThunk(
    'chat/sendMessageToPrivateChat',
    async ({ user1Id, user2Id, message }, { rejectWithValue }) => {
        try {
            await sendService(user1Id, user2Id, message);
        } catch (error) {
            return rejectWithValue(error.message);
        }
    }
);

const chatSlice = createSlice({
    name: 'chat',
    initialState,
    reducers: {
        receiveMessage: (state, action) => {
            const { userName, message } = action.payload;
            const chatRoom = state.currentChatRoom;

            if (chatRoom) {
                if (!state.messages[chatRoom]) {
                    state.messages[chatRoom] = [];
                }
                state.messages[chatRoom].push({ userName, message });
            }
        },
    },
    extraReducers: (builder) => {
        builder
            .addCase(connectToChatHub.fulfilled, (state, action) => {
                state.connectionStatus = action.payload.connectionStatus;
            })
            .addCase(connectToChatHub.rejected, (state, action) => {
                state.connectionStatus = 'disconnected';
                state.error = action.payload;
            })
            .addCase(joinPrivateChat.fulfilled, (state, action) => {
                state.currentChatRoom = action.payload;
            })
            .addCase(joinPrivateChat.rejected, (state, action) => {
                state.error = action.payload;
            })
            .addCase(sendMessageToPrivateChat.rejected, (state, action) => {
                state.error = action.payload;
            });
    },
});

export const { receiveMessage } = chatSlice.actions;

export default chatSlice.reducer;

import { createAsyncThunk, createSlice } from '@reduxjs/toolkit';
import { connectToChatHub as connectService, joinPrivateChat as joinService, sendMessageToPrivateChat as sendService, getConnection } from '../../services/chatService';

const loadMessagesFromLocalStorage = () => {
    const messages = {};
    for (let key in localStorage) {
        if (localStorage.hasOwnProperty(key)) {
            try {
                messages[key] = JSON.parse(localStorage.getItem(key));
            } catch (e) {
                console.error(`Ошибка при парсинге ключа ${key}:`, e);
            }
        }
    }
    return messages;
};

const initialState = {
    connectionStatus: 'disconnected',
    messages: loadMessagesFromLocalStorage(),
    currentChatRoom: null,
    status: 'idle',
    error: null,
};

export const connectToChatHub = createAsyncThunk(
    'chat/connectToChatHub',
    async (_, { dispatch, rejectWithValue }) => {
        try {
            return await connectService(dispatch, receiveMessage);
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

export const setChats = createAsyncThunk(
    'chat/setChats',
    async (chats, { rejectWithValue }) => {
        try {
            return chats;
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
            const { userName, message, chatRoom } = action.payload;
            const chatRoomName = chatRoom || state.currentChatRoom;

            if (chatRoomName) {
                if (!state.messages[chatRoomName]) {
                    state.messages[chatRoomName] = [];
                }
                state.messages[chatRoomName].push({ userName, message });
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
            })
            .addCase(setChats.fulfilled, (state, action) => {
                const chats = action.payload;
                chats.forEach(chat => {
                    state.messages[chat.chatRoomName] = chat.messages.map(msg => ({
                        userName: msg.senderUsername, // Предполагается, что у вас есть senderUsername
                        message: {
                            text: msg.text,
                            file: msg.file,
                            timestamp: msg.timestamp,
                        },
                    }));
                });
            })
            .addCase(setChats.rejected, (state, action) => {
                state.error = action.payload;
            });
    },
});

export const { receiveMessage } = chatSlice.actions;

export default chatSlice.reducer;

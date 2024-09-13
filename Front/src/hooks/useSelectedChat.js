import { useSelector } from 'react-redux';

export const useSelectedChat = () => {
    const selectedChatId = useSelector((state) => state.selectChat.selectedChatId);
    const bodyName = useSelector((state) => state.selectChat.bodyName);

    return { selectedChatId, bodyName };
};

import { useRef, useState} from "react";
import { useDispatch } from 'react-redux';
import style from './MessagePanel.module.scss';
import AddImg from '../../../../../assets/img/Add.svg';
import SmileImg from '../../../../../assets/img/Smile.svg';
import SendImg from '../../../../../assets/img/Send.svg';
import { sendMessageToPrivateChat } from "../../../../../store/slices/chatSlice";
import { useSelectedChat } from "../../../../../hooks/useSelectedChat";

export const MessagePanel = () => {

    const startMessage = 'Написать сообщение...';
    const [placeholder, setPlaceholder] = useState(startMessage);
    const [message, setMessage] = useState('');
    const [filePaths, setFilePaths] = useState([]);
    const { selectedChatId } = useSelectedChat();
    const fileInputRef = useRef(null);
    const dispatch = useDispatch();

    const handleSendMessage = async () => {
        if (message.trim() || filePaths) {
            try {
                const messageObject = {
                    text: message.trim(),
                    file: filePaths || [],
                };

                await dispatch(sendMessageToPrivateChat({
                    user1Id: parseInt(JSON.parse(localStorage.getItem('user')).id),
                    user2Id: selectedChatId,
                    message: messageObject,
                })).unwrap();
                console.log(messageObject);

                setMessage('');
                setFilePaths([])
            } catch (error) {
                console.error('Error sending message:', error);
            }
        }
    };

    const sendFile = async (file) => {
        const formData = new FormData();
        formData.append("file", file);

        const response = await fetch("http://localhost:5000/file/upload", {
            method: "POST",
            body: formData
        });

        const data = await response.json();
        filePaths.push(data.filePath);
    };

    const onSendFile = (e) => {
        const file = e.target.files[0];
        if (file) {
            sendFile(file);
        }
        console.log(filePaths);

    };

    return (
        <div className={style.message_panel}>
            <div className={style.input_container}>
                <input
                    className={style.message_input}
                    placeholder={placeholder}
                    value={message}
                    onChange={(e) => setMessage(e.target.value)}
                    onFocus={() => setPlaceholder('')}
                    onBlur={() => setPlaceholder(startMessage)}
                    type="text"
                    onKeyPress={(e) => e.key === 'Enter' && handleSendMessage()}
                />
                <div className={style.nav_message_bar}>
                    <input type="file" ref={fileInputRef} style={{display: 'none'}} onChange={onSendFile}/>
                    <img id="addImg" src={AddImg} onClick={() => fileInputRef.current.click()} alt=""/>
                    <img id="smileImg" src={SmileImg} alt=""/>
                    <img id="sendImg" src={SendImg} alt="" onClick={handleSendMessage}/>
                </div>
            </div>
        </div>
    );
};

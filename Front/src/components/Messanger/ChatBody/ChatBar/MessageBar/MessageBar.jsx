import style from './MessageBar.module.scss';
import { useSelector } from 'react-redux';
import { useEffect } from "react";

export const MessageBar = () => {
    const messages = useSelector(state => state.chat.messages);
    const currentChatRoom = useSelector(state => state.chat.currentChatRoom);

    useEffect(() => {
        if (currentChatRoom && messages[currentChatRoom]) {
            localStorage.setItem(currentChatRoom, JSON.stringify(messages[currentChatRoom]));
        }
    }, [currentChatRoom, messages]);

    return (
        <div className={style.message_bar}>
            {messages[currentChatRoom] && messages[currentChatRoom].length > 0 ? (
                messages[currentChatRoom].map((msg, index) => (
                    <div key={index} className={style.message}>
                        {msg.message.file && Array.isArray(msg.message.file) && (
                            <div>
                                {msg.message.file.map((file, fileIndex) => (
                                    <div key={fileIndex}>
                                        {/\.(jpeg|jpg|gif|png|svg|webp)$/i.test(file) ? (
                                            <img src={file} alt={`file-${fileIndex + 1}`} />
                                        ) : (
                                            <a
                                                href={file}
                                                className={style.text}
                                                target="_blank"
                                                rel="noopener noreferrer"
                                            >
                                                Скачать файл {fileIndex + 1}
                                            </a>
                                        )}
                                    </div>
                                ))}
                            </div>
                        )}
                        {msg.message.text && <div className={style.text}>{msg.message.text}</div>}
                    </div>
                ))
            ) : (
                <div className={style.no_messages}>
                    Нет сообщений в этом чате.
                </div>
            )}
        </div>
    );
};

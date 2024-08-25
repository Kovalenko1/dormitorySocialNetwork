import style from './ChatBody.module.scss';
import {Header} from "./Header/Header";
import {ChatBar} from "./ChatBar/ChatBar";

export const ChatBody = () => {
    return (
        <section className={style.chat_body}>
            <Header />
            <ChatBar />
        </section>
    )
}
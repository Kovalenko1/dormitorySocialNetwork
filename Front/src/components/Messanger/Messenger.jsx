import styles from './Messenger.module.scss';
import {Sidebar} from "./Sidebar/Sidebar";
import {ChatBody} from "./ChatBody/ChatBody";

export const Messenger = () => {
    return (
        <section className={styles.messenger}>
            <Sidebar/>
            <ChatBody/>
        </section>

    );
};
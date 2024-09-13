import styles from './Messenger.module.scss';
import {Sidebar} from "./Sidebar/Sidebar";
import {ChatBody} from "./ChatBody/ChatBody";
import {useEffect} from "react";
import {connectToChatHub} from "../../store/slices/chatSlice";
import {useDispatch} from "react-redux";

export const Messenger = () => {
    const dispatch = useDispatch();

    useEffect(() => {
        dispatch(connectToChatHub());
    }, [dispatch]);



    return (
        <section className={styles.messenger}>
            <Sidebar/>
            <ChatBody/>
        </section>

    );
};
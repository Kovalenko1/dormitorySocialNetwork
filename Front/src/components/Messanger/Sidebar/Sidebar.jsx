import styles from './Sidebar.module.scss';
import SearchImg from '../../../assets/img/Search.svg';

export const Sidebar = () => {
    return (
        <aside className={styles.chat_sidebar}>
            <div className={styles.search_bar_container}>
                <div className={styles.search_bar}>
                    <div className={styles.search_img_container}>
                        <img className={styles.search_img} src={SearchImg} alt=""/>
                    </div>
                </div>
            </div>
            <div className={styles.chat_list_and_folders}>
                <div className={styles.folders_container}>
                    <div className={styles.folders}>
                        <div className={styles.chats_folder}>Чаты</div>
                        <div className={styles.groups_folder}>Группы</div>
                    </div>
                </div>
                <div className={styles.chat_list}>

                </div>
            </div>
        </aside>
    );
}
import { logout } from '../../store/slices/authSlice';
import {useDispatch} from "react-redux";
import { useNavigate } from 'react-router-dom';
import styles from './Sidebar.module.scss';
import MessengerImg from '../.././assets/img/Messenger.svg'
import NotifyImg from '../.././assets/img/Notify.svg'
import AnonProfileImg from '../../assets/img/AnonProfile.png'
import OptionsImg from '../.././assets/img/Options.svg'

export const Sidebar = () => {
    const dispatch = useDispatch();
    const navigate = useNavigate();
    return (
        <aside className={styles.sidebar}>
            <div className={styles.imagBarTop}>
                <img className={styles.img} src={MessengerImg} alt=""/>
                <img className={styles.img} src={NotifyImg} alt=""/>
            </div>
            <div className={styles.imagBarBottom}>
                <img className={styles.profileImg} src={AnonProfileImg} alt=""/>
                <img className={styles.img} src={OptionsImg} alt="" onClick={()=>{
                    dispatch(logout());
                    navigate('/auth');
                }}/>
            </div>
        </aside>
    );
}
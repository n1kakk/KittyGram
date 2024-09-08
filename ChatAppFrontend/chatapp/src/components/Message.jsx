import { Portal, MenuButton, Menu, MenuItem, MenuList, MenuGroup, MenuDivider } from "@chakra-ui/react";
import React, { useState, useEffect } from "react";
import { MdContentCopy } from "react-icons/md";
import { TfiLocationArrow } from "react-icons/tfi";
import { MdOutlineEdit } from "react-icons/md";
import { AiOutlineDelete } from "react-icons/ai";
import moment from 'moment-timezone';


export const Message = ({ messageInfo, currentUser, deleteMessage, sayMeow, editMessage }) => {
    const isCurrentUser = messageInfo.senderNickname === currentUser;
    const [isMenuOpen, setIsMenuOpen] = useState(false);
    const [isDeleteHovered, setIsDeleteHovered] = useState(false);



    //const [isEditing, setIsEditing] = useState(false);
    //const [editedMessage, setEditedMessage] = useState(messageInfo);

    const handleEditClick = () => {
        //setIsEditing(true);
        editMessage(messageInfo);
        setIsMenuOpen(false);
    };

    // const handleEditCancel = () => setIsEditing(false);
    // const handleEditChange = (e) => setEditedMessage(e.target.value);
    // const handleEditSubmit = () => {
    //     // Отправка запроса на сервер
    //     EditMessage(messageInfo.id, editedMessage);
    //     // Обновление UI
    //     messageInfo.messageContent = editedMessage;
    //     setIsEditing(false);
    // };



    const handleMenuOpen = () => setIsMenuOpen(true);
    const handleMenuClose = () => setIsMenuOpen(false);

    const handleDeleteMessage = () => {
        deleteMessage(messageInfo);
        setIsMenuOpen(false);
    };

    const handleSayMeow =() => {
        sayMeow();
        setIsMenuOpen(false);
    }

    const handleCopyMessage = (messageContent) => {
        navigator.clipboard.writeText(messageContent);
    }

    const toLocalTimeString = (dbTimeString) => {
        const date = moment.tz(dbTimeString, "UTC").tz(moment.tz.guess());
        return date.format('HH:mm');
    };
    

    return (
        <div className={`chat ${isCurrentUser ? 'chat-end' : 'chat-start'}`}>
            <div>
                <Menu>
                    <MenuButton
                        as="div"
                        className="chat-bubble text-[#4e464e] bg-[#f6ebf3] flex items-center justify-center relative"
                        _hover={{ bg: '#e0e0e0' }}
                        onClick={handleMenuOpen}
                    >
                        {messageInfo.messageContent}
                        <div className="chat-footer opacity-50 text-xs flex gap-1 items-right">
                        {toLocalTimeString(messageInfo.created)}
                        </div>
                        {isMenuOpen && (
                            <Portal>
                                <div onMouseLeave={handleMenuClose}>
                                    <MenuList >
                                        <MenuItem icon={<MdContentCopy style={{ fontSize: '15px' }}/>} onClick={() => handleCopyMessage(messageInfo.messageContent)}>Copy</MenuItem>
                                        <MenuItem icon={<MdOutlineEdit style={{ fontSize: '15px' }}/>} onClick={handleEditClick}>Edit</MenuItem>
                                        <MenuItem icon={
                                            <div style={{ display: 'flex', alignItems: 'center' }}>
                                                <TfiLocationArrow  />
                                                <TfiLocationArrow style={{ transform: 'scaleX(-1)' }} />
                                            </div>
                                        } onClick={handleSayMeow}>Say Meow</MenuItem>
                                        <MenuDivider />
                                        <MenuItem icon={<AiOutlineDelete style={{ fontSize: '15px' }}/>} onClick={handleDeleteMessage}>Delete</MenuItem>

                                    </MenuList>
                                </div>
                            </Portal>
                        )}
                    </MenuButton>
                </Menu>
            </div>
            <div className="hat-image avatar">
                <div className="w-10 rounded-full">
                    <img
                        alt="Tailwind CSS chat bubble component"
                        src="https://img.daisyui.com/images/stock/photo-1534528741775-53994a69daeb.jpg" />
                </div>
            </div>
        </div>
    );
};

export default Message;

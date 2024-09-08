import React from 'react';
import { Dropdown, Menu } from "antd";
import { MdContentCopy } from "react-icons/md";
import { RiReplyLine } from "react-icons/ri";
import { RiShareForwardLine } from "react-icons/ri";
import { AiOutlineDelete } from "react-icons/ai";

const menu = <Menu 
    onClick={({key}) => {
        console.log("click on ", key);
    }}
    items={[
    {
        label: "Copy",
        key: "copy",
        icon: <MdContentCopy />,
    },
    {
        label: "Forward",
        key: "forward",
        icon: <RiShareForwardLine />,
    },
    {
        label: "Reply",
        key: "reply",
        icon: <RiReplyLine />,
    },
    {
        label: "Delete",
        key: "delete",
        icon: <AiOutlineDelete />,
        danger: true,
        children: [
            {
              key: 'deletForYourself',
              label: 'Delete for yourself',
            },
            {
              key: 'deleteForEveryone',
              label: 'Delete for everyone',
            },
          ],
    },
]}>

</Menu>


const MessageDropdown = () => {
  return (
    <Dropdown overlay={menu}
    //trigger={["contextMenu"]}}
    trigger={["click"]}
    >
      <div className="dropdown-trigger">⋮</div>
    </Dropdown>
  )
}

export default MessageDropdown;


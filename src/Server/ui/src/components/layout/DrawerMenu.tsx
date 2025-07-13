import { Link as RouterLink } from 'react-router-dom';
import Drawer from '@mui/material/Drawer';
import Toolbar from '@mui/material/Toolbar';
import Divider from '@mui/material/Divider';
import List from '@mui/material/List';
import ListItem from '@mui/material/ListItem';
import ListItemButton from '@mui/material/ListItemButton';
import ListItemIcon from '@mui/material/ListItemIcon';
import ListItemText from '@mui/material/ListItemText';
import SupportAgentIcon from '@mui/icons-material/SupportAgent';
import ToggleOffIcon from '@mui/icons-material/ToggleOff';
import { Icon } from '@mui/material';
import MemoryIcon from '@mui/icons-material/Memory';
import PolicyIcon from '@mui/icons-material/Policy';
import SpaceDashboardIcon from '@mui/icons-material/SpaceDashboard';

interface DrawerMenuProps {
  drawerWidth: number;
}

const firstList = [
  {
    text: 'Dashboard',
    path: '/dashboard',
    icon: <Icon component={SpaceDashboardIcon} />
  },
  { 
    text: 'Agents', 
    path: '/agents',
    icon: <Icon component={SupportAgentIcon} />
 },
  { 
    text: 'Configurations',
    path: '/configurations',
    icon: <Icon component={ToggleOffIcon} />
  },
  { 
    text: 'Processes',
    path: '/processes',
    icon: <Icon component={MemoryIcon} />
  },
  { 
    text: 'Policies',
    path: '/policies',
    icon: <Icon component={PolicyIcon} />
  }
];


export default function DrawerMenu({ drawerWidth }: DrawerMenuProps) {
  return (
    <Drawer
      sx={{
        width: drawerWidth,
        flexShrink: 0,
        '& .MuiDrawer-paper': {
          width: drawerWidth,
          boxSizing: 'border-box',
        },
      }}
      variant="permanent"
      anchor="left"
    >
      <Toolbar />
      <Divider />
      <List>
        {firstList.map(({ text, path, icon }, index) => (
          <ListItem key={index} disablePadding>
            <ListItemButton component={RouterLink} to={path}>
              <ListItemIcon>
                {icon}
              </ListItemIcon>
              <ListItemText primary={text} />
            </ListItemButton>
          </ListItem>
        ))}
      </List>
    </Drawer>
  );
}

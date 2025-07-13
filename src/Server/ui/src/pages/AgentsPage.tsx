import Table from '@mui/material/Table';
import TableBody from '@mui/material/TableBody';
import TableCell from '@mui/material/TableCell';
import TableContainer from '@mui/material/TableContainer';
import TableHead from '@mui/material/TableHead';
import TableRow from '@mui/material/TableRow';
import Box from '@mui/material/Box';
import Button from '@mui/material/Button';
import Paper from '@mui/material/Paper';
import { useEffect, useState } from 'react';

interface Agent {
  id: string;
  name: string;
  isConnected: boolean;
  configurationId: string;
}

export default function AgentsPage() {
  const [agents, setAgents] = useState<Agent[]>([]);

  useEffect(() => {
    fetch('https://localhost:7041/api/agents')
      .then(response => response.json())
      .then(data => {
        setAgents(data);
      })
  }, []);

  return (
    <>
      <Box sx={{ display: 'flex', mb:2 }}>
        <Button variant="contained" color="primary" onClick={() => alert('Add Agent')}>
          Add Agent
        </Button>
      </Box>      
      <TableContainer component={Paper}>
        <Table aria-label="agents table">
          <TableHead>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Name</TableCell>
              <TableCell>Configuration ID</TableCell>
              <TableCell>Is connected</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {agents.map((agent) => (
              <TableRow key={agent.id}>
                <TableCell>{agent.id}</TableCell>
                <TableCell>{agent.name}</TableCell>
                <TableCell>{agent.configurationId}</TableCell>
                <TableCell>
                    {agent.isConnected ? 
                        <span style={{ color: 'green' }}>Yes</span> :
                        <span style={{ color: 'red' }}>No</span>}
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </>
  );
}

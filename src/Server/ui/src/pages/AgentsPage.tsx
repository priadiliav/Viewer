import {
  Table, TableBody, TableCell, TableContainer,
  TableHead, TableRow, Box, Button, Paper, TextField, IconButton
} from '@mui/material';
import { useEffect, useState } from 'react';
import BasicModal from '../components/BasicModal';
import type { Configuration } from './ConfigurationsPage';
import Autocomplete from '@mui/material/Autocomplete';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';

export interface Agent {
  id: string;
  name: string;
  isConnected: boolean;
  configurationId: string;
}

interface AgentCreateRequest {
  name: string;
  configurationId: string;
}

interface AgentUpdateRequest {
  id: string;
  name: string;
  configurationId: string;
}


export default function AgentsPage() {
  const [agents, setAgents] = useState<Agent[]>([]);
  const [configurations, setConfigurations] = useState<Configuration[]>([]);

  const [modalOpen, setModalOpen] = useState(false);
  const [editMode, setEditMode] = useState(false);

  const [formAgent, setFormAgent] = useState<AgentCreateRequest | AgentUpdateRequest>({
    name: '',
    configurationId: ''
  });

  useEffect(() => {
    fetchAgents();
  }, []);

  const fetchAgents = () => {
    fetch('https://localhost:7041/api/agents')
      .then(res => res.json())
      .then(data => setAgents(data));
  };

  const fetchConfigurations = () => {
    fetch('https://localhost:7041/api/configurations')
      .then(res => res.json())
      .then(data => setConfigurations(data));
  };

  const handleAddAgent = () => {
    if (!formAgent.name || !formAgent.configurationId) {
      alert('Please fill in all fields');
      return;
    }

    fetch('https://localhost:7041/api/agents', {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(formAgent),
    })
      .then(res => res.json())
      .then(_ => {
        closeModal();
        fetchAgents();
      });
  };

  const handleUpdateAgent = () => {    
    const { id, ...updateData } = formAgent as AgentUpdateRequest;
    if (!updateData.name || !updateData.configurationId) {
      alert('Please fill in all fields');
      return;
    }

    fetch(`https://localhost:7041/api/agents/${id}`, {
      method: 'PUT',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify(updateData),
    })
      .then(res => res.json())
      .then(_ => {
        closeModal();
        fetchAgents();
      });
  };

  const handleDeleteAgent = (id: string) => {
    if (!window.confirm('Are you sure you want to delete this agent?')) return;

    fetch(`https://localhost:7041/api/agents/${id}`, { method: 'DELETE' })
      .then(_ => fetchAgents());
  };

  const openEditModal = (agent: Agent) => {
    fetchConfigurations();
    setFormAgent({
      id: agent.id,
      name: agent.name,
      configurationId: agent.configurationId,
    });
    setEditMode(true);
    setModalOpen(true);
  };

  const openCreateModal = () => {
    setFormAgent({ name: '', configurationId: '' });
    setEditMode(false);
    setModalOpen(true);
  };

  const closeModal = () => {
    setModalOpen(false);
    setEditMode(false);
    setFormAgent({ name: '', configurationId: '' });
  };

  return (
    <>
      <Box sx={{ display: 'flex', mb: 2 }}>
        <Button variant="contained" onClick={openCreateModal}>
          Add Agent
        </Button>
      </Box>

      <BasicModal open={modalOpen} onClose={closeModal} title={editMode ? 'Edit Agent' : 'Add Agent'}>
        <Box component="form" sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
          <TextField
            label="Agent Name"
            value={formAgent.name}
            onChange={(e) => setFormAgent({ ...formAgent, name: e.target.value })}
            required
          />

          <Autocomplete
            options={configurations}
            getOptionLabel={(option) => option.name}
            value={configurations.find(c => c.id === formAgent.configurationId) || null}
            onChange={(_, value) => {
              if (value) {
                setFormAgent({ ...formAgent, configurationId: value.id });
              }
            }}
            renderOption={(props, option) => (
              <li {...props} key={option.id}>
                {option.name}
              </li>
            )}
            renderInput={(params) => <TextField {...params} label="Select Configuration" />}
            onOpen={fetchConfigurations}
          />

          <Button variant="contained" onClick={editMode ? handleUpdateAgent : handleAddAgent}>
            {editMode ? 'Update Agent' : 'Save Agent'}
          </Button>
        </Box>
      </BasicModal>

      <TableContainer component={Paper}>
        <Table>
          <TableHead>
            <TableRow>
              <TableCell>ID</TableCell>
              <TableCell>Name</TableCell>
              <TableCell>Configuration ID</TableCell>
              <TableCell>Is connected</TableCell>
              <TableCell>Actions</TableCell>
            </TableRow>
          </TableHead>
          <TableBody>
            {agents.map((agent) => (
              <TableRow key={agent.id}>
                <TableCell>{agent.id}</TableCell>
                <TableCell>{agent.name}</TableCell>
                <TableCell>{agent.configurationId}</TableCell>
                <TableCell>
                  <span style={{ color: agent.isConnected ? 'green' : 'red' }}>
                    {agent.isConnected ? 'Yes' : 'No'}
                  </span>
                </TableCell>
                <TableCell>
                  <IconButton color="primary" onClick={() => openEditModal(agent)}>
                    <EditIcon />
                  </IconButton>
                  <IconButton color="error" onClick={() => handleDeleteAgent(agent.id)}>
                    <DeleteIcon />
                  </IconButton>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </TableContainer>
    </>
  );
}

import { useEffect, useState } from "react";
import {
  Table, TableBody, TableCell, TableContainer,
  TableHead, TableRow, Box, Button, Paper, TextField, IconButton
} from '@mui/material';
import type { Policy } from "./PolicyPage";
import type { Process } from "./ProcessPage";
import Autocomplete from '@mui/material/Autocomplete';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import BasicModal from "../components/BasicModal";
import SystemUpdateIcon from '@mui/icons-material/SystemUpdate';

export interface Configuration {
    id: string;
    name: string;
    isApplied: boolean;
    policyIds: string[];
    processIds: string[];
}

export interface ConfigurationCreateRequest {
  name: string;
  policyIds: string[];
  processIds: string[];
}

export interface ConfigurationUpdateRequest {
  id: string;
  name: string;
  policyIds: string[];
  processIds: string[];
}

export default function ConfigurationsPage() {
    const apiUrl = import.meta.env.VITE_API_URL;
    
    const [configurations, setConfigurations] = useState<Configuration[]>([]);
    const [policies, setPolicies] = useState<Policy[]>([]);
    const [processes, setProcesses] = useState<Process[]>([]);

    const [modalOpen, setModalOpen] = useState(false);
    const [editMode, setEditMode] = useState(false);

    const [formConfiguration, setFormConfiguration] = useState<ConfigurationCreateRequest | ConfigurationUpdateRequest>({
        name: '',
        policyIds: [],
        processIds: [],
    });

    useEffect(() => {
        fetchConfigurations();
    }, []);

    const fetchConfigurations = async () => {
        fetch(`${apiUrl}/api/configurations`)
        .then(response => response.json())
        .then(data => {setConfigurations(data)})
    };

    const fetchPolicies = () => {
        fetch(`${apiUrl}/api/policies`)
        .then(res => res.json())
        .then(data => setPolicies(data));
    }

    const fetchProcesses = () => {
        fetch(`${apiUrl}/api/processes`)
        .then(res => res.json())
        .then(data => setProcesses(data));
    }

    const handleAddConfiguration = () => {
        if(!formConfiguration.name){
            alert('Please fill in all fields');
            return;
        }
        fetch(`${apiUrl}/api/configurations`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(formConfiguration)
        })
        .then(res => res.json())
        .then(_ => {
            closeModal();
            fetchConfigurations();
        });
    }

    const handleUpdateConfiguration = () => {
        const { id, ...updateData } = formConfiguration as ConfigurationUpdateRequest;
        if(!updateData.name){
            alert('Please fill in all fields');
            return;
        }
        fetch(`${apiUrl}/api/configurations/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updateData)
        })
        .then(res => res.json())
        .then(_ => {
            closeModal();
            fetchConfigurations();
        });
    }

    const handleDeleteConfiguration = (id: string) => {
        fetch(`${apiUrl}/api/configurations/${id}`, {
            method: 'DELETE'
        })
        .then(() => {
            fetchConfigurations();
        });
    }

    const handleApplyConfiguration = (id: string) => {
        fetch(`${apiUrl}/api/configurations/${id}/apply`, {
            method: 'POST'
        })
        .then(() => {
            fetchConfigurations();
        });
    }

    const openEditModal = (configuration: Configuration) => {
        fetchPolicies();
        fetchProcesses();
        setFormConfiguration({
            id: configuration.id,
            name: configuration.name,
            policyIds: configuration.policyIds,
            processIds: configuration.processIds,
        });
        setEditMode(true);
        setModalOpen(true);
    }

    const openCreateModal = () => {
        setFormConfiguration({ name: '', policyIds: [], processIds: [] });
        setEditMode(false);
        setModalOpen(true);
    }        

    const closeModal = () => {
        setModalOpen(false);
        setEditMode(false);
        setFormConfiguration({ name: '', policyIds: [], processIds: [] });
    };

    return (
        <>
            <Box sx={{ display: 'flex', mb:2 }}>
                <Button variant="contained" color="primary" onClick={() => openCreateModal()}>
                    Add Configuration
                </Button>
            </Box> 
            <BasicModal open={modalOpen} onClose={closeModal} title={editMode ? 'Edit Configuration' : 'Add Configuration'}>
                <Box component="form" sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                  <TextField
                    label="Configuration Name"
                    value={formConfiguration.name}
                    onChange={(e) => setFormConfiguration({ ...formConfiguration, name: e.target.value })}
                    required
                  />
                
                <Autocomplete
                    multiple
                    options={policies}
                    getOptionLabel={(option) => option.name}
                    value={policies.filter(p => formConfiguration.policyIds.includes(p.id))}
                    onChange={(_, value) => {
                        setFormConfiguration({
                        ...formConfiguration,
                        policyIds: value.map(p => p.id)
                        });
                    }}
                    onOpen={fetchPolicies}
                    renderInput={(params) => <TextField {...params} label="Policies" />}
                    />

                    <Autocomplete
                    multiple
                    options={processes}
                    getOptionLabel={(option) => option.name}
                    value={processes.filter(p => formConfiguration.processIds.includes(p.id))}
                    onChange={(_, value) => {
                        setFormConfiguration({
                        ...formConfiguration,
                        processIds: value.map(p => p.id)
                        });
                    }}
                    onOpen={fetchProcesses}
                    renderInput={(params) => <TextField {...params} label="Processes" />}
                    />

                  <Button variant="contained" onClick={editMode ? handleUpdateConfiguration : handleAddConfiguration}>
                    {editMode ? 'Update Configuration' : 'Save Configuration'}
                  </Button>
                </Box>
              </BasicModal>

            <TableContainer component={Paper}>
                <Table aria-label="configurations table">
                <TableHead>
                    <TableRow>
                        <TableCell>ID</TableCell>
                        <TableCell>Name</TableCell>
                        <TableCell>Count of processes</TableCell>
                        <TableCell>Count of policies</TableCell>
                        <TableCell>Actions</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {configurations.map((configuration) => (
                    <TableRow key={configuration.id}>
                        <TableCell>{configuration.id}</TableCell>
                        <TableCell>{configuration.name}</TableCell>
                        <TableCell>{configuration.processIds.length}</TableCell>
                        <TableCell>{configuration.policyIds.length}</TableCell>
                        <TableCell>
                            {!configuration.isApplied ? (
                                <IconButton color="primary" onClick={() => handleApplyConfiguration(configuration.id)}>
                                    <SystemUpdateIcon/>
                                </IconButton>
                            ):null}
                            <IconButton color="primary" onClick={() => openEditModal(configuration)}>
                                <EditIcon />
                            </IconButton>
                            <IconButton color="error" onClick={() => handleDeleteConfiguration(configuration.id)}>
                                <DeleteIcon />
                            </IconButton>
                        </TableCell>
                    </TableRow>))}
                </TableBody>
                </Table>
            </TableContainer>
        </>
    );
}

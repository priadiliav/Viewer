import {
  Table, TableBody, TableCell, TableContainer,
  TableHead, TableRow, Box, Button, Paper, TextField, IconButton,
} from '@mui/material';
import { useEffect, useState } from 'react';
import BasicModal from '../components/BasicModal';
import { Select, MenuItem, InputLabel, FormControl } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';

export interface Process {
    id: string;
    name: string;
    status: number;
}

interface ProcessCreateRequest {
    name: string;
    status: number;
}

interface ProcessUpdateRequest {
    id: string;
    name: string;
    status: number;
}


export default function ProcessPage() {
    const apiUrl = import.meta.env.VITE_API_URL;
    
    const [processes, setProcesses] = useState<Process[]>([]);
    
    const [modalOpen, setModalOpen] = useState(false);
    const [editMode, setEditMode] = useState(false);

    const [processStatuses, setProcessStatuses] = useState<{ key: string; value: number }[]>([]);

    const [formProcess, setFormProcess] = useState<ProcessCreateRequest>({
        name: '',
        status: 0
    });

    useEffect(() => {
        fetchEnums();
        fetchProcesses();    
    }, []);

    const fetchProcesses = () => {
        fetch(`${apiUrl}/api/processes`)
            .then(response => response.json())
            .then(data => setProcesses(data))
    };

    const fetchEnums = () => {
        fetch(`${apiUrl}/api/enums`)
            .then(response => response.json())

            .then(data => {
                console.log(data);
                setProcessStatuses(data.processStatuses)
            });
    };

    const handleAddProcess = () => {
        if(!formProcess.name || formProcess.status === undefined) {
            alert('Please fill in all fields');
            return;
        }
        fetch(`${apiUrl}/api/processes`, {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(formProcess)
        })
        .then(response => {
            if (response.ok) {
                closeModal();
                fetchProcesses();
            } else {
                alert('Failed to add process');
            }
        });
    }

    const handleUpdateProcess = () => {
        const { id, ...updateData } = formProcess as ProcessUpdateRequest;
        if(!formProcess.name || formProcess.status === undefined) {
            alert('Please fill in all fields');
            return;
        }
        fetch(`${apiUrl}/api/processes/${id}`, {
            method: 'PUT',
            headers: {
                'Content-Type': 'application/json'
            },
            body: JSON.stringify(updateData)
        })
        .then(response => {
            if (response.ok) {
                closeModal();
                fetchProcesses();
            } else {
                alert('Failed to update process');
            }
        });
    }

    const handleDeleteProcess = (id: string) => {
        fetch(`${apiUrl}/api/processes/${id}`, {
            method: 'DELETE'
        })
        .then(response => {
            if (response.ok) {
                fetchProcesses();
            } else {
                alert('Failed to delete process');
            }
        });
    }

    const closeModal = () => {
        setModalOpen(false);
        setEditMode(false);
        setFormProcess({ name: '', status: 0 });
    }  

    const openCreateModal = () => {
        setFormProcess({ 
            name: '', 
            status: 0 
        } as ProcessCreateRequest);

        setEditMode(false);
        setModalOpen(true);
    }

    const openEditModal = (process: Process) => {
        setFormProcess({ 
            id: process.id,
            name: process.name, 
            status: process.status 
        } as ProcessUpdateRequest);

        setEditMode(true);
        setModalOpen(true);
    }

    return (
        <>
            <Box sx={{ display: 'flex', mb: 2 }}>
                <Button variant="contained" color="primary" onClick={() => openCreateModal()}>
                    Add Process
                </Button>
            </Box>
            <BasicModal open={modalOpen} onClose={closeModal} title={editMode ? 'Edit Process' : 'Add Process'}>
                <Box component="form" sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                    <TextField
                        label="Process Name"
                        value={formProcess.name}
                        onChange={(e) => setFormProcess({ ...formProcess, name: e.target.value })}
                        required
                    />
                    <FormControl fullWidth>
                    <InputLabel>Registry Key Type</InputLabel>
                    <Select
                        value={formProcess.status}
                        onChange={(e) => setFormProcess({ ...formProcess, status: Number(e.target.value) })}
                        required
                    >
                        {processStatuses.map((type) => (
                            <MenuItem key={type.key} value={type.value}>
                                {type.key}
                            </MenuItem>
                        ))}
                    </Select>
                    </FormControl>


                    <Button variant="contained" color="primary" onClick={editMode ? handleUpdateProcess : handleAddProcess}>
                        {editMode ? 'Update Process' : 'Add Process'}
                    </Button>
                </Box>
            </BasicModal>
            <TableContainer component={Paper}>
                <Table>
                    <TableHead>
                        <TableRow>
                            <TableCell>Process Name</TableCell>
                            <TableCell>Status</TableCell>
                            <TableCell>Actions</TableCell>
                        </TableRow>
                    </TableHead>
                    <TableBody>
                        {processes.map((process) => (
                            <TableRow key={process.id}>
                                <TableCell>{process.name}</TableCell>
                                <TableCell>
                                    {processStatuses.find(type => Number(type.value) === process.status)?.key || 'Unknown'}
                                </TableCell>
                                <TableCell>
                                    <IconButton color="primary" onClick={() => openEditModal(process)}>
                                        <EditIcon />
                                    </IconButton>
                                    <IconButton  color="error" onClick={() => handleDeleteProcess(process.id)}>
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

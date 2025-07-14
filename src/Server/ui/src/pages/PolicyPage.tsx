import {
  Table, TableBody, TableCell, TableContainer,
  TableHead, TableRow, Box, Button, Paper, TextField, IconButton
} from '@mui/material';
import { useEffect, useState } from 'react';
import BasicModal from '../components/BasicModal';
import { Select, MenuItem, InputLabel, FormControl } from '@mui/material';
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';

export interface Policy {
    id: string;
    name: string;
    description: string;
    registryPath: string;
    registryKeyType: number;
    registryKey: string;
    registryValueType: number;
    registryValue: string;
}

interface PolicyCreateRequest {
    name: string;
    description: string;
    registryPath: string;
    registryKeyType: number;
    registryKey: string;
    registryValueType: number;
    registryValue: string;
}

interface PolicyUpdateRequest {
    id: string;
    name: string;
    description: string;
    registryPath: string;
    registryKeyType: number;
    registryKey: string;
    registryValueType: number;
    registryValue: string;
}

export default function PolicyPage() {
    const [policies, setPolicies] = useState<Policy[]>([]);
    
    const [modalOpen, setModalOpen] = useState(false);
    const [editMode, setEditMode] = useState(false);

    const [registryKeyTypes, setRegistryKeyTypes] = useState<{ key: string; value: number }[]>([]);
    const [registryValueTypes, setRegistryValueTypes] = useState<{ key: string; value: number }[]>([]);


    const [formPolicy, setFormPolicy] = useState<PolicyCreateRequest | PolicyUpdateRequest>({
        name: '',
        description: '',
        registryPath: '',
        registryKeyType: 0,
        registryKey: '',
        registryValueType: 0,
        registryValue: ''
    });

    useEffect(() => {
        fetchEnums();
        fetchPolicies();
    }, []);

    const fetchPolicies = () => {
        fetch('https://localhost:7041/api/policies')
            .then(res => res.json())
            .then(data => setPolicies(data));
    };

    const fetchEnums = () => {
    fetch('https://localhost:7041/api/enums')
        .then(res => res.json())
        .then(data => {
            console.log(data);
        setRegistryKeyTypes(data.registryKeyTypes);
        setRegistryValueTypes(data.registryTypes);
        });
    };

    const handleAddPolicy = () => {
        if (!formPolicy.name || !formPolicy.description || !formPolicy.registryPath  
            || !formPolicy.registryKey 
            || !formPolicy.registryValue) {

            console.log(formPolicy);
            alert('Please fill in all fields');
            return;
        }

        fetch('https://localhost:7041/api/policies', {
            method: 'POST',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(formPolicy),
        })
            .then(res => res.json())
            .then(_ => {
                closeModal();
                fetchPolicies();
            });
    };

    const handleUpdatePolicy = () => {
        const { id, ...updateData } = formPolicy as PolicyUpdateRequest;
        if (!updateData.name || !updateData.description || !updateData.registryPath || 
            !updateData.registryKey || 
           !updateData.registryValue) {
            console.log(updateData);
            alert('Please fill in all fields');
            return;
        }

        fetch(`https://localhost:7041/api/policies/${id}`, {
            method: 'PUT',
            headers: { 'Content-Type': 'application/json' },
            body: JSON.stringify(updateData),
        })
            .then(res => res.json())
            .then(_ => {
                closeModal();
                fetchPolicies();
            });
    };

    const handleDeletePolicy = (id: string) => {
        fetch(`https://localhost:7041/api/policies/${id}`, {
            method: 'DELETE',
        })
            .then(_ => {
                fetchPolicies();
            });
    };

    const closeModal = () => {
        setModalOpen(false);
        setEditMode(false);
        setFormPolicy({
            name: '',
            description: '',
            registryPath: '',
            registryKeyType: 0,
            registryKey: '',
            registryValueType: 0,
            registryValue: ''
        });
    };

    const openEditModal = (policy: Policy) => {
        setFormPolicy({
            id: policy.id,
            name: policy.name,
            description: policy.description,
            registryPath: policy.registryPath,
            registryKeyType: policy.registryKeyType,
            registryKey: policy.registryKey,
            registryValueType: policy.registryValueType,
            registryValue: policy.registryValue
        });
        setEditMode(true);
        setModalOpen(true);
    };

    const openCreateModal = () => {
        setFormPolicy({
            name: '',
            description: '',
            registryPath: '',
            registryKeyType: 0,
            registryKey: '',
            registryValueType: 0,
            registryValue: ''
        });
        setEditMode(false);
        setModalOpen(true);
    };

    return (
        <>
            <Box sx={{ display: 'flex', mb:2 }}>
                <Button variant="contained" color="primary" onClick={() => openCreateModal()}>
                    Add Policy
                </Button>
            </Box> 
            <BasicModal open={modalOpen} onClose={closeModal} title={editMode ? 'Edit Policy' : 'Add Policy'}>
                <Box component="form" sx={{ display: 'flex', flexDirection: 'column', gap: 2 }}>
                <TextField
                    label="Policy Name"
                    value={formPolicy.name}
                    onChange={(e) => setFormPolicy({ ...formPolicy, name: e.target.value })}
                    required
                />

                <TextField
                    label="Description"
                    value={formPolicy.description}
                    onChange={(e) => setFormPolicy({ ...formPolicy, description: e.target.value })}
                    required
                />

                <TextField
                    label="Registry Path"
                    value={formPolicy.registryPath}
                    onChange={(e) => setFormPolicy({ ...formPolicy, registryPath: e.target.value })}
                    required
                />

                <FormControl fullWidth>
                    <InputLabel>Registry Key Type</InputLabel>
                    <Select
                        value={formPolicy.registryKeyType}
                        onChange={(e) => setFormPolicy({ ...formPolicy, registryKeyType: Number(e.target.value) })}
                        required
                    >
                        {registryKeyTypes.map((type) => (
                            <MenuItem key={type.key} value={type.value}>
                                {type.key}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>

                <TextField
                    label="Registry Key"
                    value={formPolicy.registryKey}
                    onChange={(e) => setFormPolicy({ ...formPolicy, registryKey: e.target.value })}
                    required
                />

                <FormControl fullWidth>
                    <InputLabel>Registry Value Type</InputLabel>
                    <Select
                        value={formPolicy.registryValueType}
                        onChange={(e) => setFormPolicy({ ...formPolicy, registryValueType: Number(e.target.value) })}
                        required
                    >
                        {registryValueTypes.map((type) => (
                            <MenuItem key={type.key} value={type.value}>
                                {type.key}
                            </MenuItem>
                        ))}
                    </Select>
                </FormControl>

                <TextField
                    label="Registry Value"
                    value={formPolicy.registryValue}
                    onChange={(e) => setFormPolicy({ ...formPolicy, registryValue: e.target.value })}
                    required
                />

                <Button variant="contained" onClick={editMode ? handleUpdatePolicy : handleAddPolicy}>
                    {editMode ? 'Update Policy' : 'Save Policy'}
                </Button>
                </Box>
            </BasicModal>
            <TableContainer component={Paper}>
                <Table aria-label="policies table">
                <TableHead>
                    <TableRow>
                    <TableCell>ID</TableCell>
                    <TableCell>Name</TableCell>
                    <TableCell>Description</TableCell>
                    <TableCell>Registry Path</TableCell>
                    <TableCell>Registry Key Type</TableCell>
                    <TableCell>Registry Key</TableCell>
                    <TableCell>Registry Value Type</TableCell>
                    <TableCell>Registry Value</TableCell>
                    <TableCell>Actions</TableCell>
                    </TableRow>
                </TableHead>
                <TableBody>
                    {policies.map((policy) => (
                        <TableRow key={policy.id}>
                        <TableCell>{policy.id}</TableCell>
                        <TableCell>{policy.name}</TableCell>
                        <TableCell>{policy.description}</TableCell>
                        <TableCell>{policy.registryPath}</TableCell>
                        <TableCell>
                            {registryKeyTypes.find(type => Number(type.value) === policy.registryKeyType)?.key || 'Unknown'}
                        </TableCell>
                        <TableCell>{policy.registryKey}</TableCell>
                        <TableCell>
                            {registryValueTypes.find(type => Number(type.value) === policy.registryValueType)?.key || 'Unknown'}
                        </TableCell>
                        <TableCell>{policy.registryValue}</TableCell>
                        <TableCell>
                            <IconButton color="primary" onClick={() => openEditModal(policy)}>
                                <EditIcon />
                            </IconButton>
                            <IconButton  color="error" onClick={() => handleDeletePolicy(policy.id)}>
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

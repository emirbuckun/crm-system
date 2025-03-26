const API_BASE_URL = 'http://localhost:3001/api'

export interface Customer {
  id: string
  firstName: string
  lastName: string
  email: string
  region: string
  registrationDate: Date
}

export interface NewCustomer {
  firstName: string
  lastName: string
  email: string
  region: string
  registrationDate: Date
}

export const api = {
  async login(username: string, password: string) {
    const response = await fetch(`${API_BASE_URL}/auth/login`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username, password }),
    })
    if (!response.ok) throw new Error('Login failed')
    return response.json()
  },

  async register(username: string, password: string) {
    const response = await fetch(`${API_BASE_URL}/auth/register`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ username, password }),
    })
    if (!response.ok) throw new Error('Registration failed')
    return response.json()
  },

  async getCustomers(token: string) {
    const response = await fetch(`${API_BASE_URL}/customers`, {
      headers: { Authorization: `Bearer ${token}` },
    })
    if (!response.ok) throw new Error('Failed to fetch customers')
    return response.json()
  },

  async createCustomer(token: string, customer: NewCustomer) {
    const response = await fetch(`${API_BASE_URL}/customers`, {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(customer),
    })
    if (!response.ok) throw new Error('Failed to create customer')
    return response.json()
  },

  async updateCustomer(token: string, id: number, customer: NewCustomer) {
    const response = await fetch(`${API_BASE_URL}/customers/${id}`, {
      method: 'PUT',
      headers: {
        'Content-Type': 'application/json',
        Authorization: `Bearer ${token}`,
      },
      body: JSON.stringify(customer),
    })
    if (!response.ok) throw new Error('Failed to update customer')
    return response.json()
  },

  async deleteCustomer(token: string, id: number) {
    const response = await fetch(`${API_BASE_URL}/customers/${id}`, {
      method: 'DELETE',
      headers: { Authorization: `Bearer ${token}` },
    })
    if (!response.ok) throw new Error('Failed to delete customer')
  },
}

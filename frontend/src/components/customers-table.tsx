import { useEffect, useState } from "react";
import { api, Customer } from "@/lib/api";
import {
  Table,
  TableBody,
  TableCaption,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from "@/components/ui/table";
import { Button } from "@/components/ui/button";
import { Input } from "@/components/ui/input";
import { Label } from "@/components/ui/label";

export function CustomersTable() {
  const [customers, setCustomers] = useState<Customer[]>([]);
  const [error, setError] = useState<string | null>(null);
  const [searchQuery, setSearchQuery] = useState<string>("");

  useEffect(() => {
    const fetchCustomers = async () => {
      try {
        const token = localStorage.getItem("token");
        if (!token) throw new Error("Unauthorized");
        const data = await api.getCustomers(token);
        setCustomers(data);
      } catch {
        setError("Failed to fetch customers.");
      }
    };
    fetchCustomers();
  }, []);

  const handleEdit = async (id: string) => {
    const newFirstName = prompt("Enter new first name:");
    const newLastName = prompt("Enter new last name:");
    const newEmail = prompt("Enter new email:");
    const newRegion = prompt("Enter new region:");
    const newRegistrationDate = new Date();
    if (newFirstName && newLastName && newEmail && newRegion) {
      try {
        const token = localStorage.getItem("token");
        if (!token) throw new Error("Unauthorized");
        await api.updateCustomer(token, parseInt(id, 10), {
          firstName: newFirstName, lastName: newLastName,
          email: newEmail, region: newRegion,
          registrationDate: newRegistrationDate
        });
        setCustomers((prev) =>
          prev.map((customer) =>
            customer.id === id ? {
              ...customer,
              firstName: newFirstName, lastName: newLastName,
              email: newEmail, region: newRegion,
              registrationDate: newRegistrationDate
            } : customer
          )
        );
      } catch {
        setError("Failed to update customer.");
      }
    }
  };

  const handleDelete = async (id: string) => {
    if (confirm("Are you sure you want to delete this customer?")) {
      try {
        const token = localStorage.getItem("token");
        if (!token) throw new Error("Unauthorized");
        await api.deleteCustomer(token, parseInt(id, 10));
        setCustomers((prev) => prev.filter((customer) => customer.id !== id));
      } catch {
        setError("Failed to delete customer.");
      }
    }
  };

  const handleCreate = async () => {
    const newFirstName = prompt("Enter first name:");
    const newLastName = prompt("Enter last name:");
    const newEmail = prompt("Enter email:");
    const newRegion = prompt("Enter region:");
    const newRegistrationDate = new Date();
    if (newFirstName && newLastName && newEmail && newRegion) {
      try {
        const token = localStorage.getItem("token");
        if (!token) throw new Error("Unauthorized");
        const newCustomer = await api.createCustomer(token, {
          firstName: newFirstName,
          lastName: newLastName,
          email: newEmail,
          region: newRegion,
          registrationDate: newRegistrationDate,
        });
        setCustomers((prev) => [...prev, newCustomer]);
      } catch {
        setError("Failed to create customer.");
      }
    }
  };

  const filteredCustomers = customers.filter((customer) =>
    `${customer.firstName} ${customer.lastName} ${customer.email} ${customer.region}`
      .trim()
      .toLowerCase()
      .includes(searchQuery.toLowerCase())
  );

  return (
    <div className="p-6 md:p-10">
      <h1 className="text-2xl font-bold mb-6">Customers</h1>
      <div className="flex justify-between items-center mb-6">
        <div className="flex space-x-2">
          <Button
            variant={"outline"}
            className="mb-4 px-4 py-2 right-0"
            onClick={() => window.location.reload()}
          >
            Refresh
          </Button>
          <Button
            className="mb-4 px-4 py-2 right-0"
            onClick={handleCreate}
          >
            Add
          </Button>
        </div>
        <div className="flex space-x-2 items-center mb-4">
          <Label htmlFor="search" className="sr-only">
            Search
          </Label>
          <span className="text-gray-500">Search:</span>
          <Input
            type="text"
            value={searchQuery}
            placeholder="Search customers..."
            onChange={(e) => setSearchQuery(e.target.value)}
          />
        </div>
        <Button
          variant={"secondary"}
          className="mb-4 px-4 py-2 right-0"
          onClick={() => {
            localStorage.removeItem("token");
            window.location.href = "/";
          }}
        >
          Logout
        </Button>
      </div>
      {error && <p className="text-red-500 mb-4">{error}</p>}
      <Table>
        <TableCaption>A list of customers.</TableCaption>
        <TableHeader>
          <TableRow>
            <TableHead>ID</TableHead>
            <TableHead>Name</TableHead>
            <TableHead>Email</TableHead>
            <TableHead>Region</TableHead>
            <TableHead>Registration Date</TableHead>
            <TableHead>Actions</TableHead>
          </TableRow>
        </TableHeader>
        <TableBody>
          {filteredCustomers.map((customer: Customer) => (
            <TableRow key={customer.id}>
              <TableCell>{customer.id}</TableCell>
              <TableCell>
                {customer.firstName} {customer.lastName}
              </TableCell>
              <TableCell>{customer.email}</TableCell>
              <TableCell>{customer.region}</TableCell>
              <TableCell>
                {new Date(customer.registrationDate).toLocaleString()}
              </TableCell>
              <TableCell className="flex space-x-2">
                <Button
                  variant={"outline"}
                  onClick={() => handleEdit(customer.id)}
                >
                  Edit
                </Button>
                <Button
                  variant={"destructive"}
                  onClick={() => handleDelete(customer.id)}
                >
                  Delete
                </Button>
              </TableCell>
            </TableRow>
          ))}
        </TableBody>
      </Table>
    </div>
  );
}

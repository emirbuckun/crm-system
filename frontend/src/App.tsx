import { Routes, Route } from "react-router-dom";
import { AuthForm } from "@/components/login-form";
import { CustomersTable } from "@/components/customers-table";

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<AuthForm />} />
      <Route path="/customers" element={<CustomersTable />} />
    </Routes>
  );
}

import { Routes, Route } from "react-router-dom";
import { LoginForm } from "@/components/login-form";
import { CustomersTable } from "@/components/customers-table";

export default function App() {
  return (
    <Routes>
      <Route path="/" element={<LoginForm />} />
      <Route path="/customers" element={<CustomersTable />} />
    </Routes>
  );
}

import { cn } from "@/lib/utils"
import { Button } from "@/components/ui/button"
import {
  Card,
  CardContent,
  CardDescription,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { useState } from "react";
import { api } from "@/lib/api";

export function AuthForm({
  className,
  ...props
}: React.ComponentProps<"div">) {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [error, setError] = useState<string | null>(null);
  const [isRegister, setIsRegister] = useState(false);

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    try {
      if (isRegister) {
        await api.register(username, password);
        alert("Registration successful. You can now log in.");
        setIsRegister(false);
      } else {
        const { token } = await api.login(username, password);
        localStorage.setItem("token", token);
        window.location.href = "/customers";
      }
    } catch {
      setError(isRegister ?
        "Registration failed. Please try again." :
        "Login failed. Please check your credentials.");
    }
  };

  return (
    <div className="flex min-h-screen w-full items-center justify-center p-6 md:p-10 bg-gray-50">
      <div className="w-full max-w-sm">
        <div className={cn("flex flex-col gap-6", className)} {...props}>
          <Card className="shadow-lg">
            <CardHeader>
              <CardTitle className="text-center text-xl font-bold">
                {isRegister ? "Create an account" : "Login to your account"}
              </CardTitle>
              <CardDescription className="text-center text-sm text-gray-600">
                {isRegister
                  ? "Enter your details below to create an account"
                  : "Enter your email below to login to your account"}
              </CardDescription>
            </CardHeader>
            <CardContent>
              <form onSubmit={handleSubmit}>
                <div className="flex flex-col gap-6">
                  <div className="grid gap-3">
                    <Label htmlFor="username">Username</Label>
                    <Input
                      id="username"
                      type="text"
                      value={username}
                      onChange={(e) => setUsername(e.target.value)}
                      required
                    />
                  </div>
                  <div className="grid gap-3">
                    <Label htmlFor="password">Password</Label>
                    <Input
                      id="password"
                      type="password"
                      value={password}
                      onChange={(e) => setPassword(e.target.value)}
                      required
                    />
                  </div>
                  {error && <p className="text-red-500">{error}</p>}
                  <div>
                    <Button type="submit" className="w-full">
                      {isRegister ? "Sign Up" : "Login"}
                    </Button>
                  </div>
                </div>
                <div className="mt-4 text-center text-sm">
                  {isRegister ? (
                    <>
                      Already have an account?{" "}
                      <a
                        href="#"
                        onClick={() => setIsRegister(false)}
                        className="underline underline-offset-4"
                      >
                        Login
                      </a>
                    </>
                  ) : (
                    <>
                      Don&apos;t have an account?{" "}
                      <a
                        href="#"
                        onClick={() => setIsRegister(true)}
                        className="underline underline-offset-4"
                      >
                        Sign up
                      </a>
                    </>
                  )}
                </div>
              </form>
            </CardContent>
          </Card>
        </div>
      </div>
    </div>
  );
}

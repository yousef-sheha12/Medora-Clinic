import type { ReactNode } from "react";
import Navbar from "@/components/layout/Navbar";
import Sidebar from "@/components/layout/Sidebar";

export default function Dashboard({ children }: { children: ReactNode }) {
      return (
    <div className="flex min-h-screen flex-col">
      <Navbar withSidebar />
      <div className="flex flex-1">
        <Sidebar /> 
        <main className="min-w-0 flex-1 p-4 md:p-6">{children}</main> 
      </div>
    </div>
  );
}
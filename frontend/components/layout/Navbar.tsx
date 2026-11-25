"use client"

import { useState } from "react";
import Link from "next/link";
import { LogOut, Menu } from "lucide-react";
import { Button, buttonVariants } from "@/components/ui/button";
import { Sheet, SheetContent, SheetHeader, SheetTitle } from "@/components/ui/sheet";
import { SidebarNav } from "@/components/layout/Sidebar";
import { useAuth } from "@/providers/AuthProvider";
import { ROLE_HOME, ROUTES } from "@/lib/constants";

export default function ({ withSidebar = false }: { withSidebar?:boolean }){
    const [open, setOpen] = useState(false);
    const { user, role, isAuthenticated, isLoading, logout } = useAuth();
    const homeHref = role ? ROLE_HOME[role] : ROUTES.home; 

    return (
    <header className="sticky top-0 z-40 border-b bg-background">
      <div className="flex h-14 items-center gap-3 px-4">
        {withSidebar && isAuthenticated && (
          <Button
            variant="ghost"
            size="icon"
            className="md:hidden"
            onClick={() => setOpen(true)}
            aria-label="Open menu" 
          >
            <Menu className="size-5" aria-hidden="true" />
          </Button>
        )}

        <Link href={homeHref} className="text-lg font-semibold">
          Medora Clinic
        </Link>

        <div className="ml-auto flex items-center gap-2">
          {isLoading ? null : isAuthenticated ? (
            <>
              <span className="hidden text-sm text-muted-foreground sm:inline">{user?.email}</span>
              <Button variant="outline" size="sm" onClick={logout}>
                <LogOut className="size-4" aria-hidden="true" />
                Logout
              </Button>
            </>
          ) : (
            <>
              <Link href={ROUTES.login} className={buttonVariants({ variant: "ghost", size: "sm" })}>
                Login
              </Link>
              <Link href={ROUTES.register} className={buttonVariants({ size: "sm" })}>
                Register
              </Link>
            </>
          )}
        </div>
      </div>

      {withSidebar && (
        <Sheet open={open} onOpenChange={setOpen}>
          <SheetContent side="left" className="w-64">
            <SheetHeader>
              <SheetTitle>Menu</SheetTitle>
            </SheetHeader>
            <div className="px-4">
              <SidebarNav onNavigate={() => setOpen(false)} /> 
            </div>
          </SheetContent>
        </Sheet>
      )}
    </header>
  );
}


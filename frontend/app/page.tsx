import Image from "next/image";

import Link from "next/link";
import { buttonVariants } from "@/components/ui/button";  
import Navbar from "@/components/layout/Navbar";
import Footer from "@/components/layout/Footer";
import { ROUTES } from "@/lib/constants";

export default function HomePage() {
  return (
    <div className="flex min-h-screen flex-col">
      <Navbar/>
      <main className="flex flex-1 items-center justify-center bg-gradient from-secondary to-background px-4">
        <div className="mx-auto flex max-w-2xl flex-col items-center py-16 text-center">
          <Image src="/logo.png" alt="Medora Clinic logo" width={96} height={96} priority />

          <h1 className="mt-6 text-4xl font-bold tracking-tight text-primary sm:text-5xl">
            Your health, one appointment away
          </h1>

          <p className="mt-4 text-lg text-muted-foreground">
            Find trusted doctors, book your visit online, and manage your appointments in one place.
          </p>

          <div className="mt-8 flex flex-wrap justify-center gap-3">
            <Link href={ROUTES.doctors} className={buttonVariants({ size: "lg" })}>
              Find a doctor
            </Link>
            <Link href={ROUTES.register} className={buttonVariants({ variant: "outline", size: "lg" })}>
              Create an account
            </Link>
          </div>
        </div>
      </main>
      <Footer />
    </div>
  );
}
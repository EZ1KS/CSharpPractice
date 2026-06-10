--
-- PostgreSQL database dump
--

\restrict Ozdr3qEzzNyLtDVIU7WYOaxlRKnpw4kYv58TJyrBohekK9b4Kkg0S4sFKVRp25y

-- Dumped from database version 18.3
-- Dumped by pg_dump version 18.3

-- Started on 2026-06-10 12:20:05

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET transaction_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 219 (class 1259 OID 16594)
-- Name: members; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.members (
    memberid uuid DEFAULT gen_random_uuid() NOT NULL,
    fullname character varying(100) NOT NULL,
    email character varying(100) NOT NULL,
    birthdate date NOT NULL,
    isactive boolean DEFAULT true,
    registrationdate timestamp without time zone DEFAULT CURRENT_TIMESTAMP
);


ALTER TABLE public.members OWNER TO postgres;

--
-- TOC entry 222 (class 1259 OID 16628)
-- Name: memberworkout; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.memberworkout (
    memberid uuid NOT NULL,
    workoutid uuid NOT NULL,
    attendancedate timestamp without time zone DEFAULT CURRENT_TIMESTAMP NOT NULL
);


ALTER TABLE public.memberworkout OWNER TO postgres;

--
-- TOC entry 223 (class 1259 OID 16647)
-- Name: payments; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.payments (
    paymentid uuid DEFAULT gen_random_uuid() NOT NULL,
    memberid uuid NOT NULL,
    amount numeric(10,2) NOT NULL,
    paymentdate timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
    description character varying(255),
    CONSTRAINT payments_amount_check CHECK ((amount > (0)::numeric))
);


ALTER TABLE public.payments OWNER TO postgres;

--
-- TOC entry 220 (class 1259 OID 16608)
-- Name: trainers; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.trainers (
    trainerid uuid DEFAULT gen_random_uuid() NOT NULL,
    fullname character varying(100) NOT NULL,
    specialization character varying(100),
    hiredate date DEFAULT CURRENT_DATE
);


ALTER TABLE public.trainers OWNER TO postgres;

--
-- TOC entry 221 (class 1259 OID 16617)
-- Name: workouts; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.workouts (
    workoutid uuid DEFAULT gen_random_uuid() NOT NULL,
    workoutname character varying(100) NOT NULL,
    durationminutes integer NOT NULL,
    price numeric(10,2) NOT NULL,
    CONSTRAINT workouts_price_check CHECK ((price > (0)::numeric))
);


ALTER TABLE public.workouts OWNER TO postgres;

--
-- TOC entry 4943 (class 0 OID 16594)
-- Dependencies: 219
-- Data for Name: members; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.members (memberid, fullname, email, birthdate, isactive, registrationdate) FROM stdin;
95950c8d-0d9d-4d14-a168-0b06673b4951	Иван Петров	ivan@mail.com	1990-05-12	t	2026-06-10 12:15:26.55017
94f23484-ca05-478a-bb16-4fda39adc909	Мария Сидорова	maria@mail.com	1985-08-23	t	2026-06-10 12:15:26.55017
\.


--
-- TOC entry 4946 (class 0 OID 16628)
-- Dependencies: 222
-- Data for Name: memberworkout; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.memberworkout (memberid, workoutid, attendancedate) FROM stdin;
95950c8d-0d9d-4d14-a168-0b06673b4951	5dd60a59-a3fc-42b2-896c-5fe7951168e4	2026-06-01 10:00:00
95950c8d-0d9d-4d14-a168-0b06673b4951	f1e51e8f-fc96-4d8c-94bc-122f1063f301	2026-06-03 18:00:00
94f23484-ca05-478a-bb16-4fda39adc909	b17aa070-50bc-4f08-941f-04fd49816d32	2026-06-02 09:30:00
\.


--
-- TOC entry 4947 (class 0 OID 16647)
-- Dependencies: 223
-- Data for Name: payments; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.payments (paymentid, memberid, amount, paymentdate, description) FROM stdin;
58907290-b40e-4654-95bc-4118ec3a9f40	95950c8d-0d9d-4d14-a168-0b06673b4951	1200.00	2026-06-01 12:00:00	Абонемент на 2 занятия
6611f5f6-c5e7-44cd-8f98-55cd093db092	94f23484-ca05-478a-bb16-4fda39adc909	400.00	2026-06-02 10:00:00	Разовое занятие кардио
\.


--
-- TOC entry 4944 (class 0 OID 16608)
-- Dependencies: 220
-- Data for Name: trainers; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.trainers (trainerid, fullname, specialization, hiredate) FROM stdin;
de8af97a-dca1-4782-9dbc-e1de8b2a94c7	Ольга Смирнова	Йога	2020-03-01
106b60a2-1b9d-4edd-b045-0beb596e6eef	Дмитрий Козлов	Силовой тренинг	2021-06-15
\.


--
-- TOC entry 4945 (class 0 OID 16617)
-- Dependencies: 221
-- Data for Name: workouts; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.workouts (workoutid, workoutname, durationminutes, price) FROM stdin;
5dd60a59-a3fc-42b2-896c-5fe7951168e4	Йога утро	60	500.00
f1e51e8f-fc96-4d8c-94bc-122f1063f301	Силовая	90	700.00
b17aa070-50bc-4f08-941f-04fd49816d32	Кардио	45	450.00
\.


--
-- TOC entry 4782 (class 2606 OID 16607)
-- Name: members members_email_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.members
    ADD CONSTRAINT members_email_key UNIQUE (email);


--
-- TOC entry 4784 (class 2606 OID 16605)
-- Name: members members_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.members
    ADD CONSTRAINT members_pkey PRIMARY KEY (memberid);


--
-- TOC entry 4790 (class 2606 OID 16636)
-- Name: memberworkout memberworkout_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.memberworkout
    ADD CONSTRAINT memberworkout_pkey PRIMARY KEY (memberid, workoutid, attendancedate);


--
-- TOC entry 4792 (class 2606 OID 16657)
-- Name: payments payments_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.payments
    ADD CONSTRAINT payments_pkey PRIMARY KEY (paymentid);


--
-- TOC entry 4786 (class 2606 OID 16616)
-- Name: trainers trainers_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.trainers
    ADD CONSTRAINT trainers_pkey PRIMARY KEY (trainerid);


--
-- TOC entry 4788 (class 2606 OID 16627)
-- Name: workouts workouts_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.workouts
    ADD CONSTRAINT workouts_pkey PRIMARY KEY (workoutid);


--
-- TOC entry 4793 (class 2606 OID 16637)
-- Name: memberworkout memberworkout_memberid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.memberworkout
    ADD CONSTRAINT memberworkout_memberid_fkey FOREIGN KEY (memberid) REFERENCES public.members(memberid) ON DELETE CASCADE;


--
-- TOC entry 4794 (class 2606 OID 16642)
-- Name: memberworkout memberworkout_workoutid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.memberworkout
    ADD CONSTRAINT memberworkout_workoutid_fkey FOREIGN KEY (workoutid) REFERENCES public.workouts(workoutid) ON DELETE CASCADE;


--
-- TOC entry 4795 (class 2606 OID 16658)
-- Name: payments payments_memberid_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.payments
    ADD CONSTRAINT payments_memberid_fkey FOREIGN KEY (memberid) REFERENCES public.members(memberid) ON DELETE CASCADE;


-- Completed on 2026-06-10 12:20:05

--
-- PostgreSQL database dump complete
--

\unrestrict Ozdr3qEzzNyLtDVIU7WYOaxlRKnpw4kYv58TJyrBohekK9b4Kkg0S4sFKVRp25y


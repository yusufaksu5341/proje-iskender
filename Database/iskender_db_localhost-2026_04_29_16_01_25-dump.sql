--
-- PostgreSQL database dump
--

\restrict kh9z9odpCiZvdpSGR3SPTiUNRu7FlCrmfDmUqJOKGFcHhgMUyOMOtQ9p9mXQ5gH

-- Dumped from database version 18.3
-- Dumped by pg_dump version 18.3

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
-- Name: comment_tb; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.comment_tb (
    owner_id bigint,
    comment_context text NOT NULL,
    product_id bigint,
    creation_date timestamp without time zone NOT NULL
);


ALTER TABLE public.comment_tb OWNER TO postgres;

--
-- Name: content_type_whitelist_tb; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.content_type_whitelist_tb (
    content_type character varying(64) NOT NULL,
    type_suffix text NOT NULL
);


ALTER TABLE public.content_type_whitelist_tb OWNER TO postgres;

--
-- Name: product_image_tb; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.product_image_tb (
    product_id bigint NOT NULL,
    resource_path text CONSTRAINT product_image_tb_resource_uuid_not_null NOT NULL
);


ALTER TABLE public.product_image_tb OWNER TO postgres;

--
-- Name: product_price_tb; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.product_price_tb (
    bid_date timestamp without time zone NOT NULL,
    user_id bigint,
    product_id bigint,
    price numeric NOT NULL
);


ALTER TABLE public.product_price_tb OWNER TO postgres;

--
-- Name: product_tag_tb; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.product_tag_tb (
    product_id bigint,
    tag_id bigint
);


ALTER TABLE public.product_tag_tb OWNER TO postgres;

--
-- Name: product_tb; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.product_tb (
    product_id bigint NOT NULL,
    owner_id bigint NOT NULL,
    creation_date timestamp without time zone NOT NULL,
    expiration_date timestamp without time zone NOT NULL,
    product_name character varying(128) NOT NULL,
    visible boolean DEFAULT false NOT NULL,
    starting_price numeric NOT NULL,
    current_price numeric NOT NULL,
    single_price boolean DEFAULT false NOT NULL,
    details json,
    main_image text
);


ALTER TABLE public.product_tb OWNER TO postgres;

--
-- Name: product_tb_product_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.product_tb_product_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.product_tb_product_id_seq OWNER TO postgres;

--
-- Name: product_tb_product_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.product_tb_product_id_seq OWNED BY public.product_tb.product_id;


--
-- Name: resource_tb; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.resource_tb (
    resource_uuid uuid NOT NULL,
    content_type character varying(64),
    visible boolean DEFAULT true NOT NULL
);


ALTER TABLE public.resource_tb OWNER TO postgres;

--
-- Name: tags_tb; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.tags_tb (
    tag_id bigint NOT NULL,
    tag_name text NOT NULL
);


ALTER TABLE public.tags_tb OWNER TO postgres;

--
-- Name: tags_tb_tag_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.tags_tb_tag_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.tags_tb_tag_id_seq OWNER TO postgres;

--
-- Name: tags_tb_tag_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.tags_tb_tag_id_seq OWNED BY public.tags_tb.tag_id;


--
-- Name: user_data_tb; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.user_data_tb (
    user_id bigint NOT NULL,
    user_name character varying(128) NOT NULL,
    user_mail character varying(64) NOT NULL,
    user_password character varying(256) NOT NULL,
    user_role character(1) DEFAULT 0 NOT NULL
);


ALTER TABLE public.user_data_tb OWNER TO postgres;

--
-- Name: user_data_tb_user_id_seq; Type: SEQUENCE; Schema: public; Owner: postgres
--

CREATE SEQUENCE public.user_data_tb_user_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;


ALTER SEQUENCE public.user_data_tb_user_id_seq OWNER TO postgres;

--
-- Name: user_data_tb_user_id_seq; Type: SEQUENCE OWNED BY; Schema: public; Owner: postgres
--

ALTER SEQUENCE public.user_data_tb_user_id_seq OWNED BY public.user_data_tb.user_id;


--
-- Name: user_follow_tb; Type: TABLE; Schema: public; Owner: postgres
--

CREATE TABLE public.user_follow_tb (
    user_id bigint,
    product_id bigint
);


ALTER TABLE public.user_follow_tb OWNER TO postgres;

--
-- Name: product_tb product_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.product_tb ALTER COLUMN product_id SET DEFAULT nextval('public.product_tb_product_id_seq'::regclass);


--
-- Name: tags_tb tag_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tags_tb ALTER COLUMN tag_id SET DEFAULT nextval('public.tags_tb_tag_id_seq'::regclass);


--
-- Name: user_data_tb user_id; Type: DEFAULT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_data_tb ALTER COLUMN user_id SET DEFAULT nextval('public.user_data_tb_user_id_seq'::regclass);


--
-- Data for Name: comment_tb; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.comment_tb (owner_id, comment_context, product_id, creation_date) FROM stdin;
\.


--
-- Data for Name: content_type_whitelist_tb; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.content_type_whitelist_tb (content_type, type_suffix) FROM stdin;
\.


--
-- Data for Name: product_image_tb; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.product_image_tb (product_id, resource_path) FROM stdin;
\.


--
-- Data for Name: product_price_tb; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.product_price_tb (bid_date, user_id, product_id, price) FROM stdin;
\.


--
-- Data for Name: product_tag_tb; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.product_tag_tb (product_id, tag_id) FROM stdin;
\.


--
-- Data for Name: product_tb; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.product_tb (product_id, owner_id, creation_date, expiration_date, product_name, visible, starting_price, current_price, single_price, details, main_image) FROM stdin;
1	1	2023-04-12 08:05:06	2030-04-12 08:05:06	sex machine	t	10.00	10.00	f	\N	\N
3	1	2026-04-26 19:31:31	2026-05-26 19:31:38	Sikiş Makinesi	t	20	20	t	\N	\N
\.


--
-- Data for Name: resource_tb; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.resource_tb (resource_uuid, content_type, visible) FROM stdin;
\.


--
-- Data for Name: tags_tb; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.tags_tb (tag_id, tag_name) FROM stdin;
\.


--
-- Data for Name: user_data_tb; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.user_data_tb (user_id, user_name, user_mail, user_password, user_role) FROM stdin;
1	lord_pengu	batuhantorlak@protonmail.com	sexi_martilar	0
\.


--
-- Data for Name: user_follow_tb; Type: TABLE DATA; Schema: public; Owner: postgres
--

COPY public.user_follow_tb (user_id, product_id) FROM stdin;
\.


--
-- Name: product_tb_product_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.product_tb_product_id_seq', 3, true);


--
-- Name: tags_tb_tag_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.tags_tb_tag_id_seq', 1, false);


--
-- Name: user_data_tb_user_id_seq; Type: SEQUENCE SET; Schema: public; Owner: postgres
--

SELECT pg_catalog.setval('public.user_data_tb_user_id_seq', 1, true);


--
-- Name: content_type_whitelist_tb content_type_whitelist_tb_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.content_type_whitelist_tb
    ADD CONSTRAINT content_type_whitelist_tb_pkey PRIMARY KEY (content_type);


--
-- Name: product_image_tb product_image_tb_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.product_image_tb
    ADD CONSTRAINT product_image_tb_pkey PRIMARY KEY (product_id, resource_path);


--
-- Name: product_tb product_tb_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.product_tb
    ADD CONSTRAINT product_tb_pkey PRIMARY KEY (product_id);


--
-- Name: resource_tb resource_tb_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.resource_tb
    ADD CONSTRAINT resource_tb_pkey PRIMARY KEY (resource_uuid);


--
-- Name: tags_tb tags_tb_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.tags_tb
    ADD CONSTRAINT tags_tb_pkey PRIMARY KEY (tag_id);


--
-- Name: user_data_tb user_data_tb_pkey; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_data_tb
    ADD CONSTRAINT user_data_tb_pkey PRIMARY KEY (user_id);


--
-- Name: user_data_tb user_data_tb_user_mail_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_data_tb
    ADD CONSTRAINT user_data_tb_user_mail_key UNIQUE (user_mail);


--
-- Name: user_data_tb user_data_tb_user_name_key; Type: CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_data_tb
    ADD CONSTRAINT user_data_tb_user_name_key UNIQUE (user_name);


--
-- Name: comment_tb comment_tb_owner_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.comment_tb
    ADD CONSTRAINT comment_tb_owner_id_fkey FOREIGN KEY (owner_id) REFERENCES public.user_data_tb(user_id);


--
-- Name: comment_tb comment_tb_product_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.comment_tb
    ADD CONSTRAINT comment_tb_product_id_fkey FOREIGN KEY (product_id) REFERENCES public.product_tb(product_id);


--
-- Name: product_image_tb product_image_tb_product_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.product_image_tb
    ADD CONSTRAINT product_image_tb_product_id_fkey FOREIGN KEY (product_id) REFERENCES public.product_tb(product_id);


--
-- Name: product_price_tb product_price_tb_product_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.product_price_tb
    ADD CONSTRAINT product_price_tb_product_id_fkey FOREIGN KEY (product_id) REFERENCES public.product_tb(product_id);


--
-- Name: product_price_tb product_price_tb_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.product_price_tb
    ADD CONSTRAINT product_price_tb_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.user_data_tb(user_id);


--
-- Name: product_tag_tb product_tag_tb_product_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.product_tag_tb
    ADD CONSTRAINT product_tag_tb_product_id_fkey FOREIGN KEY (product_id) REFERENCES public.product_tb(product_id);


--
-- Name: product_tag_tb product_tag_tb_tag_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.product_tag_tb
    ADD CONSTRAINT product_tag_tb_tag_id_fkey FOREIGN KEY (tag_id) REFERENCES public.tags_tb(tag_id);


--
-- Name: product_tb product_tb_owner_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.product_tb
    ADD CONSTRAINT product_tb_owner_id_fkey FOREIGN KEY (owner_id) REFERENCES public.user_data_tb(user_id);


--
-- Name: resource_tb resource_tb_content_type_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.resource_tb
    ADD CONSTRAINT resource_tb_content_type_fkey FOREIGN KEY (content_type) REFERENCES public.content_type_whitelist_tb(content_type);


--
-- Name: user_follow_tb user_follow_tb_product_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_follow_tb
    ADD CONSTRAINT user_follow_tb_product_id_fkey FOREIGN KEY (product_id) REFERENCES public.product_tb(product_id);


--
-- Name: user_follow_tb user_follow_tb_user_id_fkey; Type: FK CONSTRAINT; Schema: public; Owner: postgres
--

ALTER TABLE ONLY public.user_follow_tb
    ADD CONSTRAINT user_follow_tb_user_id_fkey FOREIGN KEY (user_id) REFERENCES public.user_data_tb(user_id);


--
-- PostgreSQL database dump complete
--

\unrestrict kh9z9odpCiZvdpSGR3SPTiUNRu7FlCrmfDmUqJOKGFcHhgMUyOMOtQ9p9mXQ5gH


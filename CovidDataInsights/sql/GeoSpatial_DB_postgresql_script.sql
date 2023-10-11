--
-- PostgreSQL database dump
--

-- Dumped from database version 16.0
-- Dumped by pg_dump version 16.0

-- Started on 2023-10-11 22:48:02

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 5927 (class 1262 OID 24578)
-- Name: GeoSpatialDB; Type: DATABASE; Schema: -; Owner: geospatialwatch
--

CREATE DATABASE "GeoSpatialDB" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE_PROVIDER = libc LOCALE = 'English_United States.1252';


ALTER DATABASE "GeoSpatialDB" OWNER TO geospatialwatch;

\connect "GeoSpatialDB"

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 5928 (class 0 OID 0)
-- Name: GeoSpatialDB; Type: DATABASE PROPERTIES; Schema: -; Owner: geospatialwatch
--

ALTER DATABASE "GeoSpatialDB" SET search_path TO '$user', 'public', 'topology';


\connect "GeoSpatialDB"

SET statement_timeout = 0;
SET lock_timeout = 0;
SET idle_in_transaction_session_timeout = 0;
SET client_encoding = 'UTF8';
SET standard_conforming_strings = on;
SELECT pg_catalog.set_config('search_path', '', false);
SET check_function_bodies = false;
SET xmloption = content;
SET client_min_messages = warning;
SET row_security = off;

--
-- TOC entry 8 (class 2615 OID 25667)
-- Name: topology; Type: SCHEMA; Schema: -; Owner: postgres
--

CREATE SCHEMA topology;


ALTER SCHEMA topology OWNER TO postgres;

--
-- TOC entry 5929 (class 0 OID 0)
-- Dependencies: 8
-- Name: SCHEMA topology; Type: COMMENT; Schema: -; Owner: postgres
--

COMMENT ON SCHEMA topology IS 'PostGIS Topology schema';


--
-- TOC entry 2 (class 3079 OID 24591)
-- Name: postgis; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS postgis WITH SCHEMA public;


--
-- TOC entry 5930 (class 0 OID 0)
-- Dependencies: 2
-- Name: EXTENSION postgis; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION postgis IS 'PostGIS geometry and geography spatial types and functions';


--
-- TOC entry 3 (class 3079 OID 25668)
-- Name: postgis_topology; Type: EXTENSION; Schema: -; Owner: -
--

CREATE EXTENSION IF NOT EXISTS postgis_topology WITH SCHEMA topology;


--
-- TOC entry 5931 (class 0 OID 0)
-- Dependencies: 3
-- Name: EXTENSION postgis_topology; Type: COMMENT; Schema: -; Owner: 
--

COMMENT ON EXTENSION postgis_topology IS 'PostGIS topology spatial types and functions';


SET default_tablespace = '';

SET default_table_access_method = heap;

--
-- TOC entry 219 (class 1259 OID 24584)
-- Name: Countries; Type: TABLE; Schema: public; Owner: geospatialwatch
--

CREATE TABLE public."Countries" (
    "Id" uuid NOT NULL,
    featurecla character varying(20) NOT NULL,
    sovereignt character varying(35) NOT NULL,
    type character varying(20) NOT NULL,
    admin character varying(40) NOT NULL,
    name_long character varying(40) NOT NULL,
    "formal_EN" character varying(55),
    "name_EN" character varying(45) NOT NULL,
    "Coordinates" text
);


ALTER TABLE public."Countries" OWNER TO geospatialwatch;

--
-- TOC entry 218 (class 1259 OID 24579)
-- Name: __EFMigrationsHistory; Type: TABLE; Schema: public; Owner: geospatialwatch
--

CREATE TABLE public."__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL
);


ALTER TABLE public."__EFMigrationsHistory" OWNER TO geospatialwatch;

--
-- TOC entry 5763 (class 2606 OID 24590)
-- Name: Countries PK_Countries; Type: CONSTRAINT; Schema: public; Owner: geospatialwatch
--

ALTER TABLE ONLY public."Countries"
    ADD CONSTRAINT "PK_Countries" PRIMARY KEY ("Id");


--
-- TOC entry 5761 (class 2606 OID 24583)
-- Name: __EFMigrationsHistory PK___EFMigrationsHistory; Type: CONSTRAINT; Schema: public; Owner: geospatialwatch
--

ALTER TABLE ONLY public."__EFMigrationsHistory"
    ADD CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId");


-- Completed on 2023-10-11 22:48:02

--
-- PostgreSQL database dump complete
--


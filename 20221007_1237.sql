PGDMP         &            	    z            SpoonsAndForks    14.5    14.4 2    $           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            %           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            &           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            '           1262    17059    SpoonsAndForks    DATABASE     m   CREATE DATABASE "SpoonsAndForks" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'Russian_Russia.1251';
     DROP DATABASE "SpoonsAndForks";
                postgres    false            ?            1259    17114    carts    TABLE     t   CREATE TABLE public.carts (
    id integer NOT NULL,
    userid bigint,
    goodid integer,
    quantity integer
);
    DROP TABLE public.carts;
       public         heap    postgres    false            ?            1259    17113    carts_id_seq    SEQUENCE     ?   CREATE SEQUENCE public.carts_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.carts_id_seq;
       public          postgres    false    216            (           0    0    carts_id_seq    SEQUENCE OWNED BY     =   ALTER SEQUENCE public.carts_id_seq OWNED BY public.carts.id;
          public          postgres    false    215            ?            1259    17085 
   categories    TABLE     ]   CREATE TABLE public.categories (
    id integer NOT NULL,
    name character varying(255)
);
    DROP TABLE public.categories;
       public         heap    postgres    false            ?            1259    17084    categories_id_seq    SEQUENCE     ?   CREATE SEQUENCE public.categories_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public.categories_id_seq;
       public          postgres    false    210            )           0    0    categories_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public.categories_id_seq OWNED BY public.categories.id;
          public          postgres    false    209            ?            1259    17092    goods    TABLE     ?   CREATE TABLE public.goods (
    id integer NOT NULL,
    name character varying(255),
    description character varying(255),
    categoryid integer,
    price numeric,
    quantity integer,
    url character varying(255)
);
    DROP TABLE public.goods;
       public         heap    postgres    false            ?            1259    17091    goods_id_seq    SEQUENCE     ?   CREATE SEQUENCE public.goods_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.goods_id_seq;
       public          postgres    false    212            *           0    0    goods_id_seq    SEQUENCE OWNED BY     =   ALTER SEQUENCE public.goods_id_seq OWNED BY public.goods.id;
          public          postgres    false    211            ?            1259    17147 
   orderitems    TABLE     ?   CREATE TABLE public.orderitems (
    id integer NOT NULL,
    orderid integer,
    goodid integer,
    price numeric,
    quantity integer
);
    DROP TABLE public.orderitems;
       public         heap    postgres    false            ?            1259    17146    orderitems_id_seq    SEQUENCE     ?   CREATE SEQUENCE public.orderitems_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 (   DROP SEQUENCE public.orderitems_id_seq;
       public          postgres    false    220            +           0    0    orderitems_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public.orderitems_id_seq OWNED BY public.orderitems.id;
          public          postgres    false    219            ?            1259    17132    orders    TABLE     w   CREATE TABLE public.orders (
    id integer NOT NULL,
    userid bigint,
    totalsum numeric,
    createddate date
);
    DROP TABLE public.orders;
       public         heap    postgres    false            ?            1259    17131    orders_id_seq    SEQUENCE     ?   CREATE SEQUENCE public.orders_id_seq
    AS integer
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 $   DROP SEQUENCE public.orders_id_seq;
       public          postgres    false    218            ,           0    0    orders_id_seq    SEQUENCE OWNED BY     ?   ALTER SEQUENCE public.orders_id_seq OWNED BY public.orders.id;
          public          postgres    false    217            ?            1259    17106    users    TABLE     H   CREATE TABLE public.users (
    id bigint NOT NULL,
    role integer
);
    DROP TABLE public.users;
       public         heap    postgres    false            ?            1259    17105    users_id_seq    SEQUENCE     u   CREATE SEQUENCE public.users_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.users_id_seq;
       public          postgres    false    214            -           0    0    users_id_seq    SEQUENCE OWNED BY     =   ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;
          public          postgres    false    213            u           2604    17088    categories id    DEFAULT     n   ALTER TABLE ONLY public.categories ALTER COLUMN id SET DEFAULT nextval('public.categories_id_seq'::regclass);
 <   ALTER TABLE public.categories ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    209    210    210            v           2604    17095    goods id    DEFAULT     d   ALTER TABLE ONLY public.goods ALTER COLUMN id SET DEFAULT nextval('public.goods_id_seq'::regclass);
 7   ALTER TABLE public.goods ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    212    211    212            x           2604    17150    orderitems id    DEFAULT     n   ALTER TABLE ONLY public.orderitems ALTER COLUMN id SET DEFAULT nextval('public.orderitems_id_seq'::regclass);
 <   ALTER TABLE public.orderitems ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    220    219    220            w           2604    17135 	   orders id    DEFAULT     f   ALTER TABLE ONLY public.orders ALTER COLUMN id SET DEFAULT nextval('public.orders_id_seq'::regclass);
 8   ALTER TABLE public.orders ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    218    217    218                      0    17114    carts 
   TABLE DATA           =   COPY public.carts (id, userid, goodid, quantity) FROM stdin;
    public          postgres    false    216   ?4                 0    17085 
   categories 
   TABLE DATA           .   COPY public.categories (id, name) FROM stdin;
    public          postgres    false    210   ?4                 0    17092    goods 
   TABLE DATA           X   COPY public.goods (id, name, description, categoryid, price, quantity, url) FROM stdin;
    public          postgres    false    212   5       !          0    17147 
   orderitems 
   TABLE DATA           J   COPY public.orderitems (id, orderid, goodid, price, quantity) FROM stdin;
    public          postgres    false    220   ?9                 0    17132    orders 
   TABLE DATA           C   COPY public.orders (id, userid, totalsum, createddate) FROM stdin;
    public          postgres    false    218   ?9                 0    17106    users 
   TABLE DATA           )   COPY public.users (id, role) FROM stdin;
    public          postgres    false    214   :       .           0    0    carts_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public.carts_id_seq', 1, false);
          public          postgres    false    215            /           0    0    categories_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.categories_id_seq', 4, true);
          public          postgres    false    209            0           0    0    goods_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public.goods_id_seq', 12, true);
          public          postgres    false    211            1           0    0    orderitems_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.orderitems_id_seq', 7, true);
          public          postgres    false    219            2           0    0    orders_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public.orders_id_seq', 2, true);
          public          postgres    false    217            3           0    0    users_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public.users_id_seq', 1, false);
          public          postgres    false    213            ?           2606    17119    carts carts_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.carts
    ADD CONSTRAINT carts_pkey PRIMARY KEY (id);
 :   ALTER TABLE ONLY public.carts DROP CONSTRAINT carts_pkey;
       public            postgres    false    216            z           2606    17090    categories categories_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public.categories
    ADD CONSTRAINT categories_pkey PRIMARY KEY (id);
 D   ALTER TABLE ONLY public.categories DROP CONSTRAINT categories_pkey;
       public            postgres    false    210            |           2606    17099    goods goods_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.goods
    ADD CONSTRAINT goods_pkey PRIMARY KEY (id);
 :   ALTER TABLE ONLY public.goods DROP CONSTRAINT goods_pkey;
       public            postgres    false    212            ?           2606    17154    orderitems orderitems_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public.orderitems
    ADD CONSTRAINT orderitems_pkey PRIMARY KEY (id);
 D   ALTER TABLE ONLY public.orderitems DROP CONSTRAINT orderitems_pkey;
       public            postgres    false    220            ?           2606    17139    orders orders_pkey 
   CONSTRAINT     P   ALTER TABLE ONLY public.orders
    ADD CONSTRAINT orders_pkey PRIMARY KEY (id);
 <   ALTER TABLE ONLY public.orders DROP CONSTRAINT orders_pkey;
       public            postgres    false    218            ~           2606    17111    users users_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);
 :   ALTER TABLE ONLY public.users DROP CONSTRAINT users_pkey;
       public            postgres    false    214            ?           2606    17100 !   goods fk_categories_id_categoryid    FK CONSTRAINT     ?   ALTER TABLE ONLY public.goods
    ADD CONSTRAINT fk_categories_id_categoryid FOREIGN KEY (categoryid) REFERENCES public.categories(id);
 K   ALTER TABLE ONLY public.goods DROP CONSTRAINT fk_categories_id_categoryid;
       public          postgres    false    212    210    3194            ?           2606    17125    carts fk_goods_id_goodid    FK CONSTRAINT     v   ALTER TABLE ONLY public.carts
    ADD CONSTRAINT fk_goods_id_goodid FOREIGN KEY (goodid) REFERENCES public.goods(id);
 B   ALTER TABLE ONLY public.carts DROP CONSTRAINT fk_goods_id_goodid;
       public          postgres    false    3196    212    216            ?           2606    17155    orderitems fk_goods_id_goodid    FK CONSTRAINT     {   ALTER TABLE ONLY public.orderitems
    ADD CONSTRAINT fk_goods_id_goodid FOREIGN KEY (goodid) REFERENCES public.goods(id);
 G   ALTER TABLE ONLY public.orderitems DROP CONSTRAINT fk_goods_id_goodid;
       public          postgres    false    220    212    3196            ?           2606    17160    orderitems fk_orders_id_orderid    FK CONSTRAINT        ALTER TABLE ONLY public.orderitems
    ADD CONSTRAINT fk_orders_id_orderid FOREIGN KEY (orderid) REFERENCES public.orders(id);
 I   ALTER TABLE ONLY public.orderitems DROP CONSTRAINT fk_orders_id_orderid;
       public          postgres    false    220    3202    218            ?           2606    17120    carts fk_users_id_userid    FK CONSTRAINT     v   ALTER TABLE ONLY public.carts
    ADD CONSTRAINT fk_users_id_userid FOREIGN KEY (userid) REFERENCES public.users(id);
 B   ALTER TABLE ONLY public.carts DROP CONSTRAINT fk_users_id_userid;
       public          postgres    false    216    214    3198            ?           2606    17140    orders fk_users_id_userid    FK CONSTRAINT     w   ALTER TABLE ONLY public.orders
    ADD CONSTRAINT fk_users_id_userid FOREIGN KEY (userid) REFERENCES public.users(id);
 C   ALTER TABLE ONLY public.orders DROP CONSTRAINT fk_users_id_userid;
       public          postgres    false    3198    214    218                  x?????? ? ?         .   x?3?.???+?2?t?/?.?2????,K-?2??I,2b???? ??         q  x??U]o?8}N~??>?J?%PZ?7J??/
?ܶZ?2???8qj;??߱Vl??JUcl??9g>?rdũʅ???? ?يJ????(HIHAD??8?ςp?w???	?Ї??J? ?4R?+	?B???)m<]@?h@??9?N??:N?u??:;?Z??ЀnOeq??F?????I?t?˰??!????s?k*?9?r*#h@8VǪ?`?5s??"J?)?????M??栵Z???{?<L??Gc[?E??7?+?xPc????`????Qʄ	?@Ƃ*?STpt$7?j)8bou{N?{쟽?{?uɛ?f}?????m??\?5????08}Kjd???T??&?4?^?,??Y$???oŠr)?V?D???2?B??t?Y0?tU#%Fi?K?S?UȈt?Y\?׀T`|U???L??d??*1?"??2ha??X9᩹?ņL#!{B?f???K?;-??l9^?{)ل??o?????v?w/?e?0N??j??5?ĵװ?k??rYI?}?Ѳ??FeB?9ͪUHR?P?X?_?<X?2LN?5????J?N6LN??c?`?????k??%c?LF?6??͙{???W?0}N??0}??|?8??TEP?DJS????B???֤??cھ?y?~$?5??N ?XZ7,?)??kτ?1?Y??q?P'?۵?W;?f???C1??????x8ǖ?W#?.c?%?4*|?kĉ65?[???G%%Q??|?Ɯ?&??t:????b?%??s??P??ܞ??,'?ù?LS?#??A(??`~JP?C?-/H??]6UՈ{?qi47FHϤ??N?k??
???????Mi??#?s????M?w>?????.v?꒙b;?8̖?[?/?4]?B? xx?S?"?uFrm??1?q}?????????>?^?f?^?z?3?f:??f+?!
?????qx/(	ŀ?)<((.u??܈?=??;???L?????yh?7?Z5??w?T???!h??yy?|S?z??=O?g??????J?σ?????HF`,??|??Dp??!n?+螶:0|?:?k3,k?)@??`,I?oM՘?'?XBf??n*؟?iЭ?????2#R???????UmnF?/$}J??S?u?>???      !   &   x?3?4BcSKNC.0?6?2?4????0?=... ?s         1   x?3??0074?000?47??4202?5??50?2B?2340F?????? @
:         /   x??I 0?7?!x??}?e???vY?Y?K?x???1     
PGDMP     .                	    z            SpoonsAndForks    14.5    14.4 6    ,           0    0    ENCODING    ENCODING        SET client_encoding = 'UTF8';
                      false            -           0    0 
   STDSTRINGS 
   STDSTRINGS     (   SET standard_conforming_strings = 'on';
                      false            .           0    0 
   SEARCHPATH 
   SEARCHPATH     8   SELECT pg_catalog.set_config('search_path', '', false);
                      false            /           1262    17059    SpoonsAndForks    DATABASE     m   CREATE DATABASE "SpoonsAndForks" WITH TEMPLATE = template0 ENCODING = 'UTF8' LOCALE = 'Russian_Russia.1251';
     DROP DATABASE "SpoonsAndForks";
                postgres    false            ?            1259    17114    carts    TABLE     ?   CREATE TABLE public.carts (
    id integer NOT NULL,
    userid bigint,
    goodid integer,
    quantity integer NOT NULL,
    CONSTRAINT positive_quantity_check CHECK ((quantity > 0))
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
       public          postgres    false    216            0           0    0    carts_id_seq    SEQUENCE OWNED BY     =   ALTER SEQUENCE public.carts_id_seq OWNED BY public.carts.id;
          public          postgres    false    215            ?            1259    17085 
   categories    TABLE     f   CREATE TABLE public.categories (
    id integer NOT NULL,
    name character varying(255) NOT NULL
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
       public          postgres    false    210            1           0    0    categories_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public.categories_id_seq OWNED BY public.categories.id;
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
       public          postgres    false    212            2           0    0    goods_id_seq    SEQUENCE OWNED BY     =   ALTER SEQUENCE public.goods_id_seq OWNED BY public.goods.id;
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
       public          postgres    false    220            3           0    0    orderitems_id_seq    SEQUENCE OWNED BY     G   ALTER SEQUENCE public.orderitems_id_seq OWNED BY public.orderitems.id;
          public          postgres    false    219            ?            1259    17132    orders    TABLE     ?   CREATE TABLE public.orders (
    id integer NOT NULL,
    userid bigint,
    totalsum numeric,
    createddate timestamp without time zone
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
       public          postgres    false    218            4           0    0    orders_id_seq    SEQUENCE OWNED BY     ?   ALTER SEQUENCE public.orders_id_seq OWNED BY public.orders.id;
          public          postgres    false    217            ?            1259    17106    users    TABLE     R   CREATE TABLE public.users (
    id bigint NOT NULL,
    role character varying
);
    DROP TABLE public.users;
       public         heap    postgres    false            ?            1259    17105    users_id_seq    SEQUENCE     u   CREATE SEQUENCE public.users_id_seq
    START WITH 1
    INCREMENT BY 1
    NO MINVALUE
    NO MAXVALUE
    CACHE 1;
 #   DROP SEQUENCE public.users_id_seq;
       public          postgres    false    214            5           0    0    users_id_seq    SEQUENCE OWNED BY     =   ALTER SEQUENCE public.users_id_seq OWNED BY public.users.id;
          public          postgres    false    213            w           2604    17181    carts id    DEFAULT     d   ALTER TABLE ONLY public.carts ALTER COLUMN id SET DEFAULT nextval('public.carts_id_seq'::regclass);
 7   ALTER TABLE public.carts ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    215    216    216            u           2604    17088    categories id    DEFAULT     n   ALTER TABLE ONLY public.categories ALTER COLUMN id SET DEFAULT nextval('public.categories_id_seq'::regclass);
 <   ALTER TABLE public.categories ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    210    209    210            v           2604    17095    goods id    DEFAULT     d   ALTER TABLE ONLY public.goods ALTER COLUMN id SET DEFAULT nextval('public.goods_id_seq'::regclass);
 7   ALTER TABLE public.goods ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    211    212    212            z           2604    17150    orderitems id    DEFAULT     n   ALTER TABLE ONLY public.orderitems ALTER COLUMN id SET DEFAULT nextval('public.orderitems_id_seq'::regclass);
 <   ALTER TABLE public.orderitems ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    220    219    220            y           2604    17135 	   orders id    DEFAULT     f   ALTER TABLE ONLY public.orders ALTER COLUMN id SET DEFAULT nextval('public.orders_id_seq'::regclass);
 8   ALTER TABLE public.orders ALTER COLUMN id DROP DEFAULT;
       public          postgres    false    217    218    218            %          0    17114    carts 
   TABLE DATA           =   COPY public.carts (id, userid, goodid, quantity) FROM stdin;
    public          postgres    false    216   ?9                 0    17085 
   categories 
   TABLE DATA           .   COPY public.categories (id, name) FROM stdin;
    public          postgres    false    210   7:       !          0    17092    goods 
   TABLE DATA           X   COPY public.goods (id, name, description, categoryid, price, quantity, url) FROM stdin;
    public          postgres    false    212   ?:       )          0    17147 
   orderitems 
   TABLE DATA           J   COPY public.orderitems (id, orderid, goodid, price, quantity) FROM stdin;
    public          postgres    false    220   ?       '          0    17132    orders 
   TABLE DATA           C   COPY public.orders (id, userid, totalsum, createddate) FROM stdin;
    public          postgres    false    218   Y?       #          0    17106    users 
   TABLE DATA           )   COPY public.users (id, role) FROM stdin;
    public          postgres    false    214   ??       6           0    0    carts_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public.carts_id_seq', 12, true);
          public          postgres    false    215            7           0    0    categories_id_seq    SEQUENCE SET     @   SELECT pg_catalog.setval('public.categories_id_seq', 13, true);
          public          postgres    false    209            8           0    0    goods_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public.goods_id_seq', 13, true);
          public          postgres    false    211            9           0    0    orderitems_id_seq    SEQUENCE SET     ?   SELECT pg_catalog.setval('public.orderitems_id_seq', 9, true);
          public          postgres    false    219            :           0    0    orders_id_seq    SEQUENCE SET     <   SELECT pg_catalog.setval('public.orders_id_seq', 15, true);
          public          postgres    false    217            ;           0    0    users_id_seq    SEQUENCE SET     ;   SELECT pg_catalog.setval('public.users_id_seq', 1, false);
          public          postgres    false    213            ?           2606    17119    carts carts_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.carts
    ADD CONSTRAINT carts_pkey PRIMARY KEY (id);
 :   ALTER TABLE ONLY public.carts DROP CONSTRAINT carts_pkey;
       public            postgres    false    216            |           2606    17167    categories categories_name_key 
   CONSTRAINT     Y   ALTER TABLE ONLY public.categories
    ADD CONSTRAINT categories_name_key UNIQUE (name);
 H   ALTER TABLE ONLY public.categories DROP CONSTRAINT categories_name_key;
       public            postgres    false    210            ~           2606    17090    categories categories_pkey 
   CONSTRAINT     X   ALTER TABLE ONLY public.categories
    ADD CONSTRAINT categories_pkey PRIMARY KEY (id);
 D   ALTER TABLE ONLY public.categories DROP CONSTRAINT categories_pkey;
       public            postgres    false    210            ?           2606    17169    goods goods_name_key 
   CONSTRAINT     O   ALTER TABLE ONLY public.goods
    ADD CONSTRAINT goods_name_key UNIQUE (name);
 >   ALTER TABLE ONLY public.goods DROP CONSTRAINT goods_name_key;
       public            postgres    false    212            ?           2606    17099    goods goods_pkey 
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
       public            postgres    false    218            ?           2606    17171    users users_id_key 
   CONSTRAINT     K   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_id_key UNIQUE (id);
 <   ALTER TABLE ONLY public.users DROP CONSTRAINT users_id_key;
       public            postgres    false    214            ?           2606    17111    users users_pkey 
   CONSTRAINT     N   ALTER TABLE ONLY public.users
    ADD CONSTRAINT users_pkey PRIMARY KEY (id);
 :   ALTER TABLE ONLY public.users DROP CONSTRAINT users_pkey;
       public            postgres    false    214            ?           2606    17100 !   goods fk_categories_id_categoryid    FK CONSTRAINT     ?   ALTER TABLE ONLY public.goods
    ADD CONSTRAINT fk_categories_id_categoryid FOREIGN KEY (categoryid) REFERENCES public.categories(id);
 K   ALTER TABLE ONLY public.goods DROP CONSTRAINT fk_categories_id_categoryid;
       public          postgres    false    210    212    3198            ?           2606    17125    carts fk_goods_id_goodid    FK CONSTRAINT     v   ALTER TABLE ONLY public.carts
    ADD CONSTRAINT fk_goods_id_goodid FOREIGN KEY (goodid) REFERENCES public.goods(id);
 B   ALTER TABLE ONLY public.carts DROP CONSTRAINT fk_goods_id_goodid;
       public          postgres    false    3202    216    212            ?           2606    17155    orderitems fk_goods_id_goodid    FK CONSTRAINT     {   ALTER TABLE ONLY public.orderitems
    ADD CONSTRAINT fk_goods_id_goodid FOREIGN KEY (goodid) REFERENCES public.goods(id);
 G   ALTER TABLE ONLY public.orderitems DROP CONSTRAINT fk_goods_id_goodid;
       public          postgres    false    220    212    3202            ?           2606    17160    orderitems fk_orders_id_orderid    FK CONSTRAINT        ALTER TABLE ONLY public.orderitems
    ADD CONSTRAINT fk_orders_id_orderid FOREIGN KEY (orderid) REFERENCES public.orders(id);
 I   ALTER TABLE ONLY public.orderitems DROP CONSTRAINT fk_orders_id_orderid;
       public          postgres    false    220    3210    218            ?           2606    17120    carts fk_users_id_userid    FK CONSTRAINT     v   ALTER TABLE ONLY public.carts
    ADD CONSTRAINT fk_users_id_userid FOREIGN KEY (userid) REFERENCES public.users(id);
 B   ALTER TABLE ONLY public.carts DROP CONSTRAINT fk_users_id_userid;
       public          postgres    false    216    214    3206            ?           2606    17140    orders fk_users_id_userid    FK CONSTRAINT     w   ALTER TABLE ONLY public.orders
    ADD CONSTRAINT fk_users_id_userid FOREIGN KEY (userid) REFERENCES public.users(id);
 C   ALTER TABLE ONLY public.orders DROP CONSTRAINT fk_users_id_userid;
       public          postgres    false    3206    214    218            %   3   x????0074?000?44?4?f?F斖?&?F@Kd??F\1z\\\ 
?         F   x?3?.???+?2?t?/?.?2????,K-?2??I,2,9CKR??3s??,8?R˝????E?\1z\\\ ??u      !   x  x??U?N?8}N?b?<?H????Rz;\J??h$?&Nb???vz9_??Nu?9	ח??Z???dũ.??M??Ҡ???w?k??sDQ(?Jhd ?
??X7 ??s?LJ`(????z?^fL??8;??^???T?g?칝LF?f??Z?????O?????	??g??L???X??d@ Cc?Ӑ???KK3?(	gf_5`??(??TRrJJ???L??p?/??????^?????I?:|?"????????,?? ,?ZCH8??AJ?3t??v?_N?\ ?V?&U?"C6?G??GI?\;?3?S@,?f&ʨ?R??t?Q2?u?l?K?]Ҝ?5?R%$???q)?Z??????yȉ ?H?Q?A??o3?/,?K? <?W??Pk?$?N(1???
͸???͖4-%?????=?s?????_xA˪a?|?j?k?7??S?????e??????F???WTT???$?1?P90?_?<X??? њ???o[?N6lL??c???ߑV?m??##?̆?6??͙??a?/0}Lnz0}??|8??TIP???? +?#S?*k???-?tu????)V?9??@?X^,?i??m?D?>/??٨W??Կr?W{?
?b??A??O!\??`4?cIր??p+XBaɄA?x916??U#????-?6?B??Ɯ????u:????r'2?Թy?߾?????,'?ya?Ċ?!?????A???{+?r??M]??ิ??GHφ??F?=??1,????~%s??I[d?(??q?w?w??t|??xu?l2?=\KlfKF?ܕ;??d?R?<>??eY@?:#?6D??<?v}?????z??{?^]g???o.5&s??{?%\s?D?Dؼ?D??O?-?:cEaō???????IV??`?I????f;??{???X?#??S?tpټ<???????{??w3?haq??҆?S???G?",3"?????{?I.?}??rW????.????????c0U??YSf??Ig?І??d?
֧=Etg=#1;~bfe@J?Wǫ??xћ????????oN??j2??R?j?s??礌KU??2s?i????C=s????s?hDu=??#x7?????3X?=??S???:???      )   4   x?3?4BcSKNC.0?6?2?4????0q??	??%??3?+????? ?o
?      '   t   x??л?0E?Z??8x$E?3K??#?]XE
?<??pF >?cDN?	;??#??b?_?c??g?nx?b??c??&&??A?As{͙?+???^?6??"???,J??"???U?      #   J   x??0074?000?tL????,.)J,?/?2461145741?t.-.??M-?236?0??45??M?KL???qqq w4	     
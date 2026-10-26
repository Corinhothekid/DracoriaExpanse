-- DRACORIA EVOLVED | BUILD 018 | TEXTURED DAWNHAVEN
-- Codea Modern | 1 ground tile = 2 metres
-- TydusAtlas: 384x1792, 4 columns x 16 rows, each frame 96x112.
-- Import into Codea Documents assets as TydusAtlas.
-- World bounds: x=-90..90, y=-90..90. Dawnhaven walls x/y +/-34.
local TILE_X,TILE_Y=48,24
local WORLD=90
local hero={x=0,y=-5,dir=1,walk=0,attack=0}
local joy=nil
local frames=nil
local atlasError=nil
local cobble=nil
local cobbleError=nil
local cobbleTiles={}
local camera={x=0,y=0}
local buildings={}
local trees={}
local decor={}
local dirBySector={7,8,1,2,3,4,5,6}
local function iso(x,y) return (x-y)*TILE_X/2,(x+y)*TILE_Y/2 end
local function rnd(x,y,s)
    local v=math.sin(x*127.1+y*311.7+s*74.7)*43758.5453
    return v-math.floor(v)
end
local function inside(x,y) return math.abs(x)<34 and math.abs(y)<34 end
local function road(x,y)
    if math.abs(x)<=3 or math.abs(y)<=3 then return true end
    if inside(x,y) then
        if math.abs(x-18)<=2 or math.abs(x+18)<=2 then return true end
        if math.abs(y-18)<=2 or math.abs(y+18)<=2 then return true end
    end
    return false
end
local function blocked(x,y)
    if math.abs(x)>WORLD or math.abs(y)>WORLD then return true end
    local ax,ay=math.abs(x),math.abs(y)
    local wall=(ax>=33 and ax<=35 and ay<=35) or (ay>=33 and ay<=35 and ax<=35)
    if wall then
        local gateNS=(ax<=4 and ay>=33)
        local gateEW=(ay<=4 and ax>=33)
        if not gateNS and not gateEW then return true end
    end
    for _,b in ipairs(buildings) do
        if x>b.x-b.w/2-.4 and x<b.x+b.w/2+.4 and
           y>b.y-b.h/2-.4 and y<b.y+b.h/2+.4 then return true end
    end
    for _,t in ipairs(trees) do
        if (x-t.x)^2+(y-t.y)^2<1.15^2 then return true end
    end
    return false
end
local function generateWorld()
    buildings={};trees={};decor={}
    -- A large planned city: central square, four districts, two ring avenues.
    for x=-28,28,9 do for y=-28,28,9 do
        if not road(x,y) and not road(x+3,y+3) and
           math.abs(x)>6 and math.abs(y)>6 then
            local w,h=5.5,5.5
            if not road(x,y) and not road(x-3,y-3) then
                buildings[#buildings+1]={x=x,y=y,w=w,h=h,
                    roof=math.floor(rnd(x,y,5)*3)+1}
            end
        end
    end end
    -- Four prominent civic buildings, kept away from the square.
    local civic={{-10,10,7,7,4},{10,10,7,7,4},{-10,-10,7,7,4},{10,-10,7,7,4}}
    for _,v in ipairs(civic) do
        buildings[#buildings+1]={x=v[1],y=v[2],w=v[3],h=v[4],roof=v[5]}
    end
    -- Woodland begins beyond a maintained clear perimeter and the gate roads.
    for x=-86,86,3 do for y=-86,86,3 do
        local ax,ay=math.abs(x),math.abs(y)
        if (ax>41 or ay>41) and not road(x,y) then
            local jitterX=(rnd(x,y,1)-.5)*2
            local jitterY=(rnd(x,y,2)-.5)*2
            local tx,ty=x+jitterX,y+jitterY
            if rnd(x,y,3)>.43 and not road(tx,ty) then
                trees[#trees+1]={x=tx,y=ty,
                    size=.8+rnd(x,y,4)*.55,
                    hue=rnd(x,y,6)}
            end
        elseif inside(x,y) and not road(x,y) and rnd(x,y,7)>.91 then
            decor[#decor+1]={x=x,y=y}
        end
    end end
end
local function loadAtlas()
    local ok,img=pcall(function() return readImage(asset.documents.TydusAtlas) end)
    if not ok or not img then atlasError="Import TydusAtlas.png into Documents assets";return end
    if img.width~=384 or img.height~=1792 then
        atlasError="TydusAtlas must be 384 x 1792 pixels";return
    end
    frames={}
    for a=1,2 do
        frames[a]={}
        for d=1,8 do
            frames[a][d]={}
            for f=1,4 do
                local row=(a-1)*8+(d-1)
                frames[a][d][f]=img:copy((f-1)*96,(15-row)*112,96,112)
            end
        end
    end
end
local function loadCobble()
    local ok,img=pcall(function() return readImage(asset.Cobblestone) end)
    if not ok or not img then
        cobbleError="Cobblestone missing: using stone-color fallback"
        return
    end
    cobble=img
    -- Pre-render a diamond-shaped tile using the actual imported texture.
    -- Source texture is sampled at a repeatable scale for every map tile.
    -- Masking outside the diamond is achieved using transparent pixels.
    local cw,ch=48,24
    for variant=1,4 do
        local tileImg=image(cw,ch)
        local offsetX=((variant-1)%2)*17
        local offsetY=math.floor((variant-1)/2)*11
        for yy=0,ch-1 do
            local dy=math.abs((yy+.5-ch/2)/(ch/2))
            local span=(1-dy)*cw/2
            for xx=0,cw-1 do
                if math.abs(xx+.5-cw/2)<=span then
                    local u=(xx+offsetX)%img.width+1
                    local v=(yy+offsetY)%img.height+1
                    local r,g,b,a=img:get(u,v)
                    tileImg:set(xx+1,yy+1,r,g,b,a or 255)
                end
            end
        end
        cobbleTiles[variant]=tileImg
    end
end
function setup()
    viewer.mode=FULLSCREEN
    generateWorld()
    loadAtlas()
    loadCobble()
end
local function fill(c) style.fill(c[1],c[2],c[3]) end
local function diamond(px,py,c)
    fill(c);style.noStroke()
    triangle(px,py+TILE_Y/2,px+TILE_X/2,py,px,py-TILE_Y/2)
    triangle(px,py+TILE_Y/2,px,py-TILE_Y/2,px-TILE_X/2,py)
end
local function ground(x,y,px,py)
    local city=inside(x,y)
    local r=road(x,y)
    local noise=rnd(math.floor(x/3),math.floor(y/3),8)
    local c
    if math.abs(x)<=7 and math.abs(y)<=7 then
        c=noise>.5 and {172,160,139} or {165,153,133}
    elseif r then c=noise>.5 and {142,130,113} or {136,124,108}
    elseif city then c=noise>.5 and {106,143,105} or {100,136,99}
    else c=noise>.5 and {73,125,83} or {69,119,78} end
    local plaza=math.abs(x)<=7 and math.abs(y)<=7
    if (r or plaza) and cobbleTiles[1] then
        -- One 48x24 diamond image per ground tile, same footprint as Build 017.
        local variant=math.floor(rnd(x,y,17)*4)+1
        sprite(cobbleTiles[variant],px,py,TILE_X,TILE_Y)
    else
        diamond(px,py,c)
    end
    if city and not r and not plaza and rnd(x,y,10)>.96 then
        style.fill(118,156,114,160);ellipse(px,py,6,3)
    end
end
local function worldToScreen(x,y)
    local px,py=iso(x,y)
    return px-camera.x+WIDTH/2,py-camera.y+HEIGHT*.51
end
local function building(b)
    local x,y=worldToScreen(b.x,b.y)
    local w=b.w*TILE_X*.52
    local h=b.h*TILE_Y*.52
    local height=b.roof==4 and 93 or 55
    style.noStroke()
    style.fill(88,79,74);rect(x-w/2,y-h/2,w,h)
    style.fill(183,167,139);rect(x-w*.44,y-h*.2,w*.88,height)
    style.fill(119,106,97);rect(x+w*.20,y-h*.2,w*.15,height)
    local roof=b.roof==1 and {105,58,55} or
        (b.roof==2 and {70,84,106} or
        (b.roof==3 and {113,96,70} or {73,76,105}))
    fill(roof)
    triangle(x-w*.56,y+height-h*.18,x,y+height+h*.42,x+w*.56,y+height-h*.18)
    style.fill(72,56,46);rect(x-11,y-h*.2,22,31)
    style.fill(220,190,119);rect(x-7,y+height*.33,14,13)
end
local function tree(t)
    local x,y=worldToScreen(t.x,t.y)
    local s=t.size
    style.noStroke()
    style.fill(59,49,40);rect(x-5*s,y,10*s,36*s)
    local c=t.hue>.5 and {37,88,56} or {44,101,62}
    fill(c);ellipse(x,y+49*s,49*s,59*s)
    style.fill(70,129,77,160);ellipse(x-10*s,y+60*s,22*s,26*s)
end
local function wallTile(x,y)
    local sx,sy=worldToScreen(x,y)
    style.fill(92,92,101);rect(sx-16,sy,32,36)
    style.fill(145,145,150);rect(sx-18,sy+30,36,9)
    style.fill(175,174,170);rect(sx-15,sy+36,9,9)
    style.fill(175,174,170);rect(sx+7,sy+36,9,9)
end
local function update(dt)
    if hero.attack>0 then hero.attack=math.max(0,hero.attack-dt) end
    if not joy or hero.attack>0 then return end
    local sx,sy=joy.dx,joy.dy
    local m=math.sqrt(sx*sx+sy*sy)
    if m<.08 then return end
    local wx=sx/24+sy/12
    local wy=sy/12-sx/24
    local wm=math.sqrt(wx*wx+wy*wy)
    if wm<.001 then return end
    wx=wx/wm*4.2*dt;wy=wy/wm*4.2*dt
    if not blocked(hero.x+wx,hero.y) then hero.x=hero.x+wx end
    if not blocked(hero.x,hero.y+wy) then hero.y=hero.y+wy end
    hero.walk=hero.walk+dt
    local angle=math.atan(sy,sx)
    local sector=math.floor((angle+math.pi/8)/(math.pi/4))%8+1
    hero.dir=dirBySector[sector]
end
local function drawHero()
    local x,y=worldToScreen(hero.x,hero.y)
    style.fill(21,38,29,115);ellipse(x,y+2,36,11)
    if frames then
        local action=hero.attack>0 and 2 or 1
        local frame=1
        if action==2 then frame=math.max(1,math.min(4,math.floor((.48-hero.attack)/.12)+1))
        elseif joy and math.abs(joy.dx)+math.abs(joy.dy)>.12 then
            frame=math.floor(hero.walk*8)%4+1
        end
        sprite(frames[action][hero.dir][frame],x,y+42,96,112)
    else
        style.fill(36,75,147);rect(x-12,y+8,24,34)
        style.fill(224,220,218);ellipse(x,y+48,22,22)
    end
end
local function renderWorld()
    local minX,maxX=math.max(-WORLD,math.floor(hero.x-29)),math.min(WORLD,math.ceil(hero.x+29))
    local minY,maxY=math.max(-WORLD,math.floor(hero.y-29)),math.min(WORLD,math.ceil(hero.y+29))
    -- Draw terrain first, then height-bearing objects by screen depth.
    for y=minY,maxY do for x=minX,maxX do
        local sx,sy=worldToScreen(x,y)
        if sx>-TILE_X and sx<WIDTH+TILE_X and sy>-TILE_Y and sy<HEIGHT+TILE_Y then
            ground(x,y,sx,sy)
        end
    end end
    local objects={}
    for _,b in ipairs(buildings) do
        if math.abs(b.x-hero.x)<31 and math.abs(b.y-hero.y)<31 then
            objects[#objects+1]={depth=b.x+b.y,kind=1,obj=b}
        end
    end
    for _,t in ipairs(trees) do
        if math.abs(t.x-hero.x)<31 and math.abs(t.y-hero.y)<31 then
            objects[#objects+1]={depth=t.x+t.y,kind=2,obj=t}
        end
    end
    for i=-34,34,2 do
        for _,w in ipairs({{i,-34},{i,34},{-34,i},{34,i}}) do
            local x,y=w[1],w[2]
            if not (math.abs(x)<=4 and math.abs(y)>=33) and
               not (math.abs(y)<=4 and math.abs(x)>=33) and
               math.abs(x-hero.x)<31 and math.abs(y-hero.y)<31 then
                objects[#objects+1]={depth=x+y,kind=3,x=x,y=y}
            end
        end
    end
    objects[#objects+1]={depth=hero.x+hero.y,kind=4}
    table.sort(objects,function(a,b) return a.depth<b.depth end)
    for _,o in ipairs(objects) do
        if o.kind==1 then building(o.obj)
        elseif o.kind==2 then tree(o.obj)
        elseif o.kind==3 then wallTile(o.x,o.y)
        else drawHero() end
    end
end
local function drawUI()
    style.fill(16,26,43,235);rect(0,HEIGHT-91,WIDTH,91)
    style.fill(242,209,144);fontSize(20);text("DAWNHAVEN",WIDTH/2,HEIGHT-25)
    style.fill(221);fontSize(13)
    text("DRACORIA EVOLVED  •  BUILD 018",WIDTH/2,HEIGHT-61)
    if atlasError then
        style.fill(150,44,50,235);rect(8,HEIGHT-142,WIDTH-16,40)
        style.fill(255);fontSize(13);text(atlasError,WIDTH/2,HEIGHT-122)
    end
    if cobbleError then
        style.fill(255,215,135);fontSize(12)
        text(cobbleError,WIDTH/2,HEIGHT-158)
    end
    local jx,jy=joy and joy.x or 83,joy and joy.y or 104
    style.fill(30,53,81,175);ellipse(jx,jy,115,115)
    style.fill(142,177,211)
    ellipse(jx+(joy and joy.dx*36 or 0),jy+(joy and joy.dy*36 or 0),45,45)
    style.fill(145,57,68);ellipse(WIDTH-80,100,96,96)
    style.fill(255);fontSize(18);text("ATTACK",WIDTH-80,100)
end
function draw()
    local dt=math.min(DeltaTime or .016,.05)
    update(dt)
    local targetX,targetY=iso(hero.x,hero.y)
    camera.x=camera.x+(targetX-camera.x)*math.min(1,dt*8)
    camera.y=camera.y+(targetY-camera.y)*math.min(1,dt*8)
    background(51,100,70)
    renderWorld()
    drawUI()
end
function touched(t)
    if t.state==BEGAN then
        if t.x>WIDTH-145 and t.y<175 then
            if hero.attack<=0 then hero.attack=.48 end
        elseif t.x<WIDTH*.58 and t.y<HEIGHT*.35 then
            joy={id=t.id,x=t.x,y=t.y,dx=0,dy=0}
        end
    elseif t.state==MOVING and joy and t.id==joy.id then
        local dx,dy=t.x-joy.x,t.y-joy.y
        local m=math.sqrt(dx*dx+dy*dy)
        local power=math.min(1,m/45)
        if m>0 then joy.dx=dx/m*power;joy.dy=dy/m*power end
    elseif (t.state==ENDED or t.state==CANCELLED) and joy and t.id==joy.id then
        joy=nil
    end
end

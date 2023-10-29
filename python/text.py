with open("C:\Git\python\TextList_2.txt", 'r', encoding='utf-8') as file:
    lines = file.readlines()
results = [line.split('\t')[0] for line in lines]
with open('C:\Git\python\output.txt', 'w', encoding='utf-8') as output_file:
    for result in results:
        output_file.write(result + '\n')